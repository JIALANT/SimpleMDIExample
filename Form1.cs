using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Drawing.Text;
namespace SimpleMDIExample
{
    public partial class Form1 : Form
    {
        private int _Num = 1;     
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tSCbBoxFontName.DropDownItems.Clear();
            InstalledFontCollection ifc = new InstalledFontCollection();
            FontFamily[] ffs = ifc.Families;
            foreach (FontFamily ff in ffs)
                tSCbBoxFontName.DropDownItems.Add(ff.GetName(1));
            LayoutMdi(MdiLayout.Cascade);
            //Text = "多文档文本编辑器";
            // WindowState = FormWindowState.Maximized;
        }

        private void 窗口层叠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
            this.窗口层叠ToolStripMenuItem.Checked = true;
            this.垂直平铺ToolStripMenuItem.Checked = false;
            this.水平平铺ToolStripMenuItem.Checked = false;
        }

        private void 水平平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal );
            this.窗口层叠ToolStripMenuItem.Checked = false;
            this.垂直平铺ToolStripMenuItem.Checked = false;
            this.水平平铺ToolStripMenuItem.Checked = true;
        }

        private void 垂直平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical );
            this.窗口层叠ToolStripMenuItem.Checked = false;
            this.垂直平铺ToolStripMenuItem.Checked = true;
            this.水平平铺ToolStripMenuItem.Checked = false;
        }
        private void NewDoc()
        {
            Frmdoc fd = new Frmdoc();
            fd.MdiParent = this;
            fd.Text = "文档" + _Num;
            //fd.WindowState = FormWindowState.Maximized;
            fd.Show();
            fd.Activate();
            _Num++;
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "RTF格式(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(* .*)|* .*";
            o.Multiselect = false;
            if (o.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    NewDoc();
                    _Num--;
                    if (o.FilterIndex == 1)
                    {
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.LoadFile(o.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.LoadFile(o.FileName, RichTextBoxStreamType.PlainText);
                    }
                    ((Frmdoc)this.ActiveMdiChild).Text = o.FileName;
                }
                catch
                {
                    MessageBox.Show("打开失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            o.Dispose();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RTF格式(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(* .*)|* .*";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (s.FilterIndex == 1)
                        {
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SaveFile(s.FileName, RichTextBoxStreamType.RichText);
                        }
                        else
                        {
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SaveFile(s.FileName, RichTextBoxStreamType.PlainText);
                            MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("保存失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                s.Dispose();
            }
        }

        private void 关闭CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (MessageBox.Show("确定要关闭当前文档吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    ((Frmdoc)this.ActiveMdiChild).Close();
                }
            }
        }
        private void tSCbBoxFontName_TextChanged(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                RichTextBox tempRTB = new RichTextBox();
                int st = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionStart;
                int len = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionLength;
                int tempRtbstart = 0;
                Font font = ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font;
                if (len <= 0 && null != font)
                {
                    ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font = new Font(tSCbBoxFontName.Text, font.Size, font.Style);
                    return;
                }
                tempRTB.Rtf = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedRtf;
                for (int i = 0; i < len; i++)
                {
                    tempRTB.Select(tempRtbstart + i, 1);
                    tempRTB.SelectionFont = new Font(tSCbBoxFontName.Text, tempRTB.SelectionFont.Size, tempRTB.SelectionFont.Style);
                }
                tempRTB.Select(tempRtbstart, len);
                ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedRtf = tempRTB.SelectedRtf;
                ((Frmdoc)this.ActiveMdiChild).rTBDoc.Select(st, len);
                ((Frmdoc)this.ActiveMdiChild).rTBDoc.Focus();
                tempRTB.Dispose();
            }
        }
        /*private void ChangeRTBFontStyle(RichTextBox rtb, FontStyle style)
        {
            if (style != FontStyle.Bold && style != FontStyle.Italic && style != FontStyle.Underline)
            {
                throw new System.InvalidProgramException("字体格式错误");
            }
            RichTextBox tempRTB = new RichTextBox();
            int curRtbStart = rtb.SelectionStart;
            int len = rtb.SelectionLength;
            int tempRtbStart = 0;
            Font font = rtb.SelectionFont;

            if (len <= 0 && font != null)
            {
                if (style == FontStyle.Bold && font.Bold || style == FontStyle.Italic && font.Italic || style == FontStyle.Underline && font.Underline)
                {
                    rtb.Font = new Font(font, font.Style ^ style);
                }
                else if (style == FontStyle.Bold && !font.Bold || style == FontStyle.Italic && !font.Italic || style == FontStyle.Underline && !font.Underline)
                {
                    rtb.Font = new Font(font, font.Style | style);
                }
                return;
            }
            tempRTB.Rtf = rtb.SelectedRtf;
            tempRTB.Select(len - 1, 1);
            Font tempFont = (Font)tempRTB.SelectionFont.Clone();
            for (int i = 0; i < len; i++)
            {
                tempRTB.Select(tempRtbStart + i, 1);
            }
            if (style == FontStyle.Bold && tempFont.Bold || style == FontStyle.Italic && tempFont.Italic || style == FontStyle.Underline && tempFont.Underline)
            {
                tempRTB.SelectionFont = new Font(tempRTB.SelectionFont, tempRTB.SelectionFont.Style ^ style);
            }
            else if (style == FontStyle.Bold && !tempFont.Bold || style == FontStyle.Italic && !tempFont.Italic || style == FontStyle.Underline && !tempFont.Underline)
            {
                tempRTB.SelectionFont = new Font(tempRTB.SelectionFont, tempRTB.SelectionFont.Style | style);
            }
            tempRTB.Select(tempRtbStart, len);
            rtb.SelectedRtf = tempRTB.SelectedRtf;
            rtb.Select(curRtbStart, len);
            rtb.Focus();
            tempRTB.Dispose();

        }
        */
        private void 粗体ToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedText != "")
                {
                    Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont;
                    if (oldfont.Bold)
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style ^ FontStyle.Bold);
                    else
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Bold);
                }
                else
                {
                    Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font;
                    if (oldfont.Bold)
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font = new
                            Font(oldfont, oldfont.Style ^ FontStyle.Bold);
                    else
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Bold);
                }
                /*  ChangeRTBFontStyle(((Frmdoc)this.ActiveMdiChild).rTBDoc, FontStyle.Bold);*/
            }
        }

        private void 斜体ToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedText != "")
                {
                    Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont;
                    if (oldfont.Italic)
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style ^ FontStyle.Italic);
                    else
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Italic);
                }
                else
                {
                    Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font;
                    if (oldfont.Italic)
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font = new
                            Font(oldfont, oldfont.Style ^ FontStyle.Italic);
                    else
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Italic);
                }
            }
                /*ChangeRTBFontStyle(((Frmdoc)this.ActiveMdiChild).rTBDoc, FontStyle.Italic);*/
        }

        private void 下划线toolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)              
                {
                    if (((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedText != "")
                    {
                        Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont;
                        if (oldfont.Underline )
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style ^ FontStyle.Underline);
                        else
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Underline);
                    }
                    else
                    {
                        Font oldfont = ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font;
                        if (oldfont.Italic)
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font = new
                                Font(oldfont, oldfont.Style ^ FontStyle.Underline);
                        else
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionFont = new Font(oldfont, oldfont.Style | FontStyle.Underline);
                    }
                }
                /*ChangeRTBFontStyle(((Frmdoc)this.ActiveMdiChild).rTBDoc, FontStyle.Underline);*/
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RTF格式(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(* .*)|* .*";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (s.FilterIndex == 1)
                        {
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SaveFile(s.FileName, RichTextBoxStreamType.RichText);
                        }
                        else
                        {
                            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SaveFile(s.FileName, RichTextBoxStreamType.PlainText);
                            MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("保存失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                s.Dispose();
            }
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "RTF格式(*.rtf)|*.rtf|文本文件(*.txt)|*.txt|所有文件(* .*)|* .*";
            o.Multiselect = false;
            if (o.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    NewDoc();
                    _Num--;
                    if (o.FilterIndex == 1)
                    {
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.LoadFile(o.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        ((Frmdoc)this.ActiveMdiChild).rTBDoc.LoadFile(o.FileName, RichTextBoxStreamType.PlainText);
                    }
                    ((Frmdoc)this.ActiveMdiChild).Text = o.FileName;
                }
                catch
                {
                    MessageBox.Show("打开失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            o.Dispose();
        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedText != "")
                {
                    ((Frmdoc)this.ActiveMdiChild).rTBDoc.Cut ();

                }
            }
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 0)
            {
                if (((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectedText != "")
                {
                   ((Frmdoc)this.ActiveMdiChild).rTBDoc.Copy ();
                                
                }
            }

        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {

            ((Frmdoc)this.ActiveMdiChild).rTBDoc.Paste();

        }

        private void tSCbBoxFontName_Click(object sender, EventArgs e)
        {         
               if(this.fontDialog1.ShowDialog()==DialogResult.OK)
               {
                 ((Frmdoc)this.ActiveMdiChild).rTBDoc.Font = this.fontDialog1.Font;
                   
                }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
          ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionAlignment = HorizontalAlignment.Left;
              
            
        }

        private void 居中toolStripButton2_Click(object sender, EventArgs e)
        {
            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionAlignment = HorizontalAlignment.Center ;
        }

        private void 右侧toolStripButton2_Click(object sender, EventArgs e)
        {
            ((Frmdoc)this.ActiveMdiChild).rTBDoc.SelectionAlignment = HorizontalAlignment.Right ;
        }

       


        



    }
}

