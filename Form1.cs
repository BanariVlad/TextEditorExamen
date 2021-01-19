using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        private string FilePath;

        public Form1()
        {
            InitializeComponent();

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilePath = string.Empty;
            richTextBox1.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Document|*.txt", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(ofd.FileName))
                    {
                        FilePath = ofd.FileName;
                        Task<string> text = sr.ReadToEndAsync();
                        richTextBox1.Text = text.Result;
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Document|*.txt", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            sw.WriteLineAsync(richTextBox1.Text);
                        }
                    }
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    sw.WriteLineAsync(richTextBox1.Text);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Document|*.txt", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        sw.WriteLineAsync(richTextBox1.Text);
                    }
                }
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.Undo();

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.Cut();
        
        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e) => richTextBox1.Copy();

        private void pasteToolStripMenuItem_Click_1(object sender, EventArgs e) => richTextBox1.Paste();

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.SelectAll();

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
            richTextBox1.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog dialog = new FontDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.Font = dialog.Font;
                }
            }
        }

        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            richTextBox1.SelectionColor = colorDialog1.Color;
        }

        private void Bold_Click(object sender, EventArgs e)
        {
            String s = richTextBox1.SelectedText;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.SelectedText = s;
        }

        private void Italic_Click(object sender, EventArgs e)
        {
            String s = richTextBox1.SelectedText;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Italic);
            richTextBox1.SelectedText = s;
        }

        private void Regular_Click(object sender, EventArgs e)
        {
            String s = richTextBox1.SelectedText;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            richTextBox1.SelectedText = s;
        }

        private void Left_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void Center_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void Right_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }
        public void CountWords(RichTextBox richBox, Label textBox)
        {
            var words = richBox.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            textBox.Text = @"Words: " + words;
        }
        public void nrpropozitii(RichTextBox richBox)
        {
     
            label7.Text = $"Prop: {richBox.Text.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Length}";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            CountWords(richTextBox1, label1);
            cuvantlung((richTextBox1));
            cuvantscurt((richTextBox1));
            nrsimboluri();
            alfabet(richTextBox1);
            nrpropozitii((richTextBox1));
            simblung((richTextBox1));
        }

        private void cuvantlung(RichTextBox text)
        {
            var words = text.Text.Split(' ');
            label2.Text = $"Cel mai lung cuvant: {words.OrderByDescending(x => x.Length).First()}";
        }
        private void cuvantscurt(RichTextBox text)
        {
            var words = text.Text.Split(' ');
            label3.Text = $"Cel mai scurt cuvant: {words.OrderByDescending(x => x.Length).Last()}";
        }

        private void nrsimboluri()
        {
            label4.Text = $"Simb: { richTextBox1.Text.Length.ToString() }"; 
        }

        private void alfabet(RichTextBox text)
        {

            label5.Text = $"Alf: {new String(text.Text.Distinct().ToArray())}";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void simblung(RichTextBox text)
        {
          var secventa=new string(text.Text.Select((c, index)=>text.Text.Substring(index).TakeWhile(e=>e==c)).OrderByDescending(e=>e.Count()).First().ToArray());
          label8.Text = secventa;
          label8.Text = $"Sec: {new string(text.Text.Select((c, index)=>text.Text.Substring(index).TakeWhile(e=>e==c)).OrderByDescending(e=>e.Count()).First().ToArray())}";

        }
    }
}
