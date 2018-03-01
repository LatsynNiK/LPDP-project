using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SLT;

namespace ПОСП
{
    public partial class Closing_Form : Form
    {
        public Closing_Form()
        {
            InitializeComponent();
        }

        public enum ResultType {Save, NoSave, Cancel}
        public ResultType Result = ResultType.Cancel;

        private void Save_button_Click(object sender, EventArgs e)
        {
            Result = ResultType.Save;
            this.Close();
        }

        private void NoSave_button_Click(object sender, EventArgs e)
        {
            Result = ResultType.NoSave;
            this.Close();
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            Result = ResultType.Cancel;
            this.Close();
        }
    }
}
