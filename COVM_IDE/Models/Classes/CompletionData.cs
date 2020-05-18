using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE
{
    public class RegisterCompletion : ICompletionData
    {
        public RegisterCompletion(string text, string des)
        {
            this.Text = text;
            this.TextDes = des;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string TextDes { get; private set; }
        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return TextDes; }
        }

        public double Priority => 0;

        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
