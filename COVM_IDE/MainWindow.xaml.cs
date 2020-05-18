using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using COVM_IDE.Models.Compiler;
using ICSharpCode.AvalonEdit.AddIn;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.SharpDevelop.Editor;
using MahApps.Metro.Controls;

namespace COVM_IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private const double FONT_MAX_SIZE = 60d;
        private const double FONT_MIN_SIZE = 5d;
        private ToolTip toolTip = new ToolTip();
        private ITextMarkerService textMarkerService;
        private CompletionWindow completionWindow;
        private static string lastEnteredChar = "";
        private Models.Compiler.Translator Compiler = new Models.Compiler.Translator();



        public MainWindow()
        {
            InitializeComponent();
            TextEdit.TextArea.TextView.LineTransformers.Add(new ColorizeAvalonEdit());


            string[] commands = {"call", "ret", "add", "sub", "div", "mul", "xor", "and", "or", "not", "inc", "dec", "ret", "mov", "push", "pop",
                                      "jmp", "cmp", "jt", "jnt", "lstart", "lend", "nop"};

            string[] registers = { "rax", "rcx", "rdx", "rbx", "rsp", "rbp", "rsi", "rdi", "eax", "ecx", "edx", "ebx", "esp", "ebp", "esi", "edi", "ax", "bx", "cx", "dx" };

            /*
             * */

            foreach (var str in commands)
            {
                ColorizedCommands.Instance.Add(new ColorizedCommands.HighlightCommand()
                {
                    command = str,
                    size = 21,
                    forecolor = System.Windows.Media.Brushes.Yellow,
                    bold = true
                });
            }

            foreach (var str in registers)
            {
                ColorizedCommands.Instance.Add(new ColorizedCommands.HighlightCommand()
                {
                    command = str,
                    size = 21,
                    forecolor = System.Windows.Media.Brushes.LightGreen,
                    bold = true
                });
            }

            ColorizedCommands.Instance.Add(new ColorizedCommands.HighlightCommand()
            {
                command = "break",
                size = 25,
                background = System.Windows.Media.Brushes.Red,
                bold = true
            });

            ColorizedCommands.Instance.Add(new ColorizedCommands.HighlightCommand()
            {
                command = "//",
                size = 25,
                background = System.Windows.Media.Brushes.Green,
            });

            var textMarkerService = new TextMarkerService(TextEdit.Document);
            TextEdit.TextArea.TextView.BackgroundRenderers.Add(textMarkerService);
            TextEdit.TextArea.TextView.LineTransformers.Add(textMarkerService);
            IServiceContainer services = (IServiceContainer)TextEdit.Document.ServiceProvider.GetService(typeof(IServiceContainer));
            if (services != null)
                services.AddService(typeof(ITextMarkerService), textMarkerService);
            this.textMarkerService = textMarkerService;

            TextEdit.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            TextEdit.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            AppendText("Output:\n", "White");
        }


        public void UpdateFontSize(bool increase)
        {
            double currentSize = TextEdit.FontSize;

            if (increase)
            {
                if (currentSize < FONT_MAX_SIZE)
                {
                    double newSize = Math.Min(FONT_MAX_SIZE, currentSize + 1);
                    TextEdit.FontSize = newSize;
                }
            }
            else
            {
                if (currentSize > FONT_MIN_SIZE)
                {
                    double newSize = Math.Max(FONT_MIN_SIZE, currentSize - 1);
                    TextEdit.FontSize = newSize;
                }
            }
        }

        private void AppendText(string text, string color)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(OutputRTB.Document.ContentEnd, OutputRTB.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                    bc.ConvertFromString(color));
                OutputRTB.ScrollToEnd();
            }
            catch (FormatException)
            {
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(2);
        }

        private void TextEdit_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool ctrl = Keyboard.Modifiers == ModifierKeys.Control;
            if (ctrl)
            {
                this.UpdateFontSize(e.Delta > 0);
                e.Handled = true;
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

            static string ByteArrayToString(byte[] ba)
            {
                return "0x" + BitConverter.ToString(ba).Replace(",", "").Replace("-", ", 0x");
            }
            string[] rtbLines = TextEdit.Text.Split(Environment.NewLine);
            Compiler.Reset();
            try
            {
                Compiler.Translate(rtbLines);

            }
            catch (Exception ex)
            {
                AppendText("Failed to compile : " + ex.Message + "\r", "red");
                return;
            }
         
            AppendText("Script compiled successfully : " + ByteArrayToString(Compiler.GetContext().ByteCode.ToArray()) + "\r", "green");

        }

       
        private void TextEdit_MouseHover(object sender, MouseEventArgs e)
        {
            var pos = TextEdit.GetPositionFromPoint(e.GetPosition(TextEdit));
            if (pos != null)
            {

                var line = pos.Value.Line;
                var column = pos.Value.Column;
                var offset = TextEdit.TextArea.Document.GetOffset(line, column);

                if (offset >= TextEdit.TextArea.Document.TextLength)
                    offset--;

                int offsetStart = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
                int offsetEnd = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

                if (offsetEnd == -1 || offsetStart == -1)
                    return;

                var currentChar = TextEdit.TextArea.Document.GetText(offset, 1);

                if (string.IsNullOrWhiteSpace(currentChar))
                    return;

                toolTip.PlacementTarget = this;
                toolTip.IsOpen = false;
                var word = TextEdit.TextArea.Document.GetText(offsetStart, offsetEnd - offsetStart);

                var des = Models.Compiler.Register.GetRegisterDescription(word);
                if (des.Length > 0)
                {
                    toolTip.IsOpen = true;
                    toolTip.Content = des;
                    e.Handled = true;
                }
                des = Models.Compiler.Instructions.GetInstructionDescription(word);
                if (des.Length > 0)
                {
                    toolTip.IsOpen = true;
                    toolTip.Content = des;
                    e.Handled = true;
                }
            }
        }

        private void TextEdit_MouseHoverStopped(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
        }

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {

            try
            {
                // register
                if (e.Text == " " && lastEnteredChar != " ")
                {
                    int offset = TextEdit.CaretOffset - 1;

                    if (offset >= TextEdit.TextArea.Document.TextLength)
                        offset--;

                    int offsetStart = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
                    int offsetEnd = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offsetStart, LogicalDirection.Forward, CaretPositioningMode.WordBorder);
                    var word = TextEdit.TextArea.Document.GetText(offsetStart, offsetEnd - offsetStart);
                    if (Models.Compiler.Instructions.IsRegisterRequired(word))
                    {
                        completionWindow = new CompletionWindow(TextEdit.TextArea);
                        IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                        foreach (var reg in Models.Compiler.Register.GetRegisters())
                            data.Add(new RegisterCompletion(reg.Key, reg.Value));
                        completionWindow.Show();
                        completionWindow.Closed += delegate
                        {
                            completionWindow = null;
                        };
                    }
                    else
                    {
                        Compiler.ScanLabels(TextEdit.Text.Split(Environment.NewLine));
                        var keywords = Compiler.ILHelper(word);
                        if (keywords.Length > 0)
                        {
                            completionWindow = new CompletionWindow(TextEdit.TextArea);
                            IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                            foreach (var reg in keywords)
                                data.Add(new RegisterCompletion(reg, reg));
                            completionWindow.Show();
                            completionWindow.Closed += delegate
                            {
                                completionWindow = null;
                            };
                        }
                    }
                }


                lastEnteredChar = e.Text;
                /*if (e.Text == ",")
                {
                    // Open code completion after the user has pressed dot:

                    int offset = TextEdit.CaretOffset - 1;

                    if (offset >= TextEdit.TextArea.Document.TextLength)
                        offset--;

                    int offsetStart = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
                    int offsetEnd = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offsetStart, LogicalDirection.Forward, CaretPositioningMode.WordBorder);
                    var word = TextEdit.TextArea.Document.GetText(offsetStart, offsetEnd - offsetStart);
                    if (Models.RegisterTranslator.RegisterTranslator.IsRegister(word))
                    {
                        completionWindow = new CompletionWindow(TextEdit.TextArea);
                        IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                        foreach (var reg in Models.RegisterTranslator.RegisterTranslator.GetRegisters())
                            data.Add(new RegisterCompletion(reg.Key, reg.Value));
                        completionWindow.Show();
                        completionWindow.Closed += delegate
                        {
                            completionWindow = null;
                        };
                    }
                }*/
            }
            catch(Exception exception)
            {

            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                    completionWindow.CompletionList.RequestInsertion(e);
            }
        }

        private void TextEdit_TextChanged(object sender, EventArgs e)
        {
            int offset = TextEdit.CaretOffset;

            if (offset >= TextEdit.TextArea.Document.TextLength)
                offset--;

            int offsetStart = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
            int offsetEnd = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offset, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

            if (offsetEnd == -1 || offsetStart == -1)
                return;



            bool IsSelected(ITextMarker marker)
            {
                int selectionEndOffset = offsetEnd;
                if (marker.StartOffset == offsetEnd)
                    return true;
                if (marker.StartOffset >= offsetStart-1 && marker.StartOffset <= selectionEndOffset)
                    return true;
                if (marker.EndOffset >= offsetStart-1 && marker.EndOffset <= selectionEndOffset)
                    return true;
                return false;
            };

            var currentChar = TextEdit.TextArea.Document.GetText(offset, 1);

            if (string.IsNullOrWhiteSpace(currentChar))
                return;

            var word = TextEdit.TextArea.Document.GetText(offsetStart, offsetEnd - offsetStart);


            var regdes = Models.Compiler.Register.GetRegisterDescription(word);
            var insdes = Models.Compiler.Instructions.GetInstructionDescription(word);
            if (regdes.Length > 0 || insdes.Length > 0)
            {
                textMarkerService.RemoveAll(IsSelected);
            }
            else
            {

                var backOffsetEnd = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, TextEdit.CaretOffset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
                var backOffsetStart = TextUtilities.GetNextCaretPosition(TextEdit.TextArea.Document, offsetStart, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
                if (!(backOffsetStart > 0 && backOffsetEnd > 0))
                {
                    if (word.Contains(' ')) return;
                    ITextMarker marker = textMarkerService.Create(offsetStart, offsetEnd - offsetStart);
                    marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
                    marker.MarkerColor = Colors.Red;
                }
                
            }
        }
    }
}
