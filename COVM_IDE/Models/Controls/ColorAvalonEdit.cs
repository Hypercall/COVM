using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.AvalonEdit.AddIn;
using System.Linq;

namespace COVM_IDE
{
    public class ColorizeAvalonEdit : DocumentColorizingTransformer
    {
        protected override void ColorizeLine(DocumentLine line)
        {
            int lineStartOffset = line.Offset;
            string text = CurrentContext.Document.GetText(line);
            int start = 0;
            int index = 0;
            foreach(ColorizedCommands.HighlightCommand command in ColorizedCommands.Instance.Get())
            {
                while ((index = text.ToLower().IndexOf(command.command.ToLower(), start, StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    if (index != text.Count() && text.ElementAt(index + 1) == ' ') break;
                    base.ChangeLinePart(
                        lineStartOffset + index,
                        lineStartOffset + index +  command.command.Length,
                        (VisualLineElement element) => {
                            Typeface tf = element.TextRunProperties.Typeface;
                            element.TextRunProperties.SetTypeface(new Typeface(
                                tf.FontFamily,
                                command.italic ? FontStyles.Italic : new FontStyle(),
                                command.bold ? FontWeights.Bold : FontWeights.Normal,
                                tf.Stretch
                            ));
                            if (command.size != 0)
                                element.TextRunProperties.SetFontHintingEmSize(command.size);
                            if(command.forecolor != null)
                                element.TextRunProperties.SetForegroundBrush(command.forecolor);
                            if (command.background != null)
                                element.TextRunProperties.SetBackgroundBrush(command.background);
                        });
                    start = index + 1; // search for next occurrence
                }

            }
            /*while ((index = text.IndexOf("AvalonEdit", start)) >= 0)
            {
                base.ChangeLinePart(
                    lineStartOffset + index, 
                    lineStartOffset + index + 10, 
                    (VisualLineElement element) => {
                    Typeface tf = element.TextRunProperties.Typeface;
                    element.TextRunProperties.SetTypeface(new Typeface(
                            tf.FontFamily,
                            FontStyles.Italic,
                            FontWeights.Bold,
                            tf.Stretch
                        ));
                    });

                start = index + 1; // search for next occurrence
            }
            index = 0;
            start = 0;
            while ((index = text.IndexOf("call", start)) >= 0)
            {

                base.ChangeLinePart(
                    lineStartOffset + index, // startOffset
                    lineStartOffset + index + 4, // endOffset
                    (VisualLineElement element) => {
                        // This lambda gets called once for every VisualLineElement
                        // between the specified offsets.
                        Typeface tf = element.TextRunProperties.Typeface;
                        // Replace the typeface with a modified version of
                        // the same typeface
                        element.TextRunProperties.SetForegroundBrush(Brushes.Red);
                        
                    });
                start = index + 1; // search for next occurrence
            }

            while ((index = text.IndexOf("breakpoint", start)) >= 0)
            {

                base.ChangeLinePart(
                    lineStartOffset + index, // startOffset
                    lineStartOffset + index + 10, // endOffset
                    (VisualLineElement element) => {
                        // This lambda gets called once for every VisualLineElement
                        // between the specified offsets.
                        Typeface tf = element.TextRunProperties.Typeface;
                        // Replace the typeface with a modified version of
                        // the same typeface
                        element.TextRunProperties.SetBackgroundBrush(Brushes.Red);

                    });
                start = index + 1; // search for next occurrence
            }*/
        }
    }
}
