
namespace StringEncrypt
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.txt_unEncrypt = new Sunny.UI.UITextBox();
            this.txt_Encrypt = new Sunny.UI.UITextBox();
            this.btn_Encrypt = new Sunny.UI.UIButton();
            this.btn_UnEncrypt = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel1.Location = new System.Drawing.Point(1, 30);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(63, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "加密前";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLabel2.Location = new System.Drawing.Point(1, 79);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(63, 23);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "加密后";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_unEncrypt
            // 
            this.txt_unEncrypt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_unEncrypt.FillColor = System.Drawing.Color.White;
            this.txt_unEncrypt.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_unEncrypt.Location = new System.Drawing.Point(61, 30);
            this.txt_unEncrypt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_unEncrypt.Maximum = 2147483647D;
            this.txt_unEncrypt.Minimum = -2147483648D;
            this.txt_unEncrypt.MinimumSize = new System.Drawing.Size(1, 1);
            this.txt_unEncrypt.Name = "txt_unEncrypt";
            this.txt_unEncrypt.Padding = new System.Windows.Forms.Padding(5);
            this.txt_unEncrypt.Size = new System.Drawing.Size(454, 25);
            this.txt_unEncrypt.TabIndex = 2;
            // 
            // txt_Encrypt
            // 
            this.txt_Encrypt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_Encrypt.FillColor = System.Drawing.Color.White;
            this.txt_Encrypt.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txt_Encrypt.Location = new System.Drawing.Point(61, 79);
            this.txt_Encrypt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_Encrypt.Maximum = 2147483647D;
            this.txt_Encrypt.Minimum = -2147483648D;
            this.txt_Encrypt.MinimumSize = new System.Drawing.Size(1, 1);
            this.txt_Encrypt.Name = "txt_Encrypt";
            this.txt_Encrypt.Padding = new System.Windows.Forms.Padding(5);
            this.txt_Encrypt.Size = new System.Drawing.Size(454, 25);
            this.txt_Encrypt.TabIndex = 3;
            // 
            // btn_Encrypt
            // 
            this.btn_Encrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Encrypt.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Encrypt.Location = new System.Drawing.Point(297, 112);
            this.btn_Encrypt.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_Encrypt.Name = "btn_Encrypt";
            this.btn_Encrypt.Size = new System.Drawing.Size(100, 24);
            this.btn_Encrypt.TabIndex = 4;
            this.btn_Encrypt.Text = "加密";
            this.btn_Encrypt.Click += new System.EventHandler(this.btn_Encrypt_Click);
            // 
            // btn_UnEncrypt
            // 
            this.btn_UnEncrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_UnEncrypt.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_UnEncrypt.Location = new System.Drawing.Point(415, 112);
            this.btn_UnEncrypt.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_UnEncrypt.Name = "btn_UnEncrypt";
            this.btn_UnEncrypt.Size = new System.Drawing.Size(100, 24);
            this.btn_UnEncrypt.TabIndex = 5;
            this.btn_UnEncrypt.Text = "解密";
            this.btn_UnEncrypt.Click += new System.EventHandler(this.btn_UnEncrypt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 148);
            this.Controls.Add(this.btn_UnEncrypt);
            this.Controls.Add(this.btn_Encrypt);
            this.Controls.Add(this.txt_Encrypt);
            this.Controls.Add(this.txt_unEncrypt);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Name = "Form1";
            this.Text = "StringEncrypt";
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox txt_unEncrypt;
        private Sunny.UI.UITextBox txt_Encrypt;
        private Sunny.UI.UIButton btn_Encrypt;
        private Sunny.UI.UIButton btn_UnEncrypt;
    }
}

