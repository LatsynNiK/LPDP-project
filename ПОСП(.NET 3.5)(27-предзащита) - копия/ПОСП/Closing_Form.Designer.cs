namespace ПОСП
{
    partial class Closing_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Closing_Form));
            this.Save_button = new System.Windows.Forms.Button();
            this.NoSave_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.Ask_to_save_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Save_button
            // 
            this.Save_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Save_button.Location = new System.Drawing.Point(12, 52);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(119, 28);
            this.Save_button.TabIndex = 0;
            this.Save_button.Text = "Сохранить";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // NoSave_button
            // 
            this.NoSave_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NoSave_button.Location = new System.Drawing.Point(137, 52);
            this.NoSave_button.Name = "NoSave_button";
            this.NoSave_button.Size = new System.Drawing.Size(119, 28);
            this.NoSave_button.TabIndex = 1;
            this.NoSave_button.Text = "Не сохранять";
            this.NoSave_button.UseVisualStyleBackColor = true;
            this.NoSave_button.Click += new System.EventHandler(this.NoSave_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Cancel_button.Location = new System.Drawing.Point(262, 52);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(119, 28);
            this.Cancel_button.TabIndex = 2;
            this.Cancel_button.Text = "Отмена";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // Ask_to_save_label
            // 
            this.Ask_to_save_label.AutoSize = true;
            this.Ask_to_save_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Ask_to_save_label.Location = new System.Drawing.Point(64, 18);
            this.Ask_to_save_label.Name = "Ask_to_save_label";
            this.Ask_to_save_label.Size = new System.Drawing.Size(284, 20);
            this.Ask_to_save_label.TabIndex = 3;
            this.Ask_to_save_label.Text = "Сохранить модель перед выходом?";
            // 
            // Closing_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 92);
            this.Controls.Add(this.Ask_to_save_label);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.NoSave_button);
            this.Controls.Add(this.Save_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Closing_Form";
            this.Text = "Завершение работы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.Button NoSave_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.Label Ask_to_save_label;
    }
}