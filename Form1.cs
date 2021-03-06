﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyLib.My;

namespace Translator
{
    public partial class Form1 : Form
    {
        #region FIELDS
        public TransLation translation;
        public bool InProcess;
        private int idx;
        private Form f;
        #endregion
        public Form1()
        {
            InitializeComponent();
            InitStatus(toolStripStatusLabel1);
            NewForm2();
            // Set default language to English.
            comboBox1.SelectedIndex = 0;
        }
        #region FORM EVENTS
        private void btnLoad_Click(object sender, EventArgs e) => Dispatch(TransLation.Load($"Language\\{comboBox1.Text}.xml"));

        private void btnSave_Click(object sender, EventArgs e)
        {
            translation.MenuItems = GetFormControls(this);
            foreach (var form in this.OwnedForms) translation.MenuItems.AddRange(GetFormControls(form));
            translation.Save($"Language\\{comboBox1.Text}.xml");
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var b = new Button() { Name = $"btnDynamic{++idx}", Text = $"Dynamic{idx}" };
            b.Location = new Point(txtFilename.Location.X, txtFilename.Location.Y + idx * 18);
            this.Controls.Add(b);
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            Status("");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!InProcess)
            {
                InProcess = true;
                btnLoad_Click(null, null);
                InProcess = false;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var name = (sender as CheckBox).Text;
            Status(translation.Message("User has changed {0}.", name));
            //
            // Add a new form to this form.
            if ((sender as CheckBox) == checkBox3)
            {
                var f = NewForm2();
                f.Show();
            }
        }

        private Form NewForm2()
        {
            if (OwnedForms.Length > 0) return OwnedForms[0];

            var f = new Form();
            var b = new Button();
            f.Name = "Form2";
            b.Name = "Button2";
            f.Text = "Configuration form";
            b.Text = "my button";
            f.Controls.Add(b);
            AddOwnedForm(f);
            return f;
        }
        #endregion
        #region METHODES
        private void Dispatch(TransLation translation)
        {
            this.translation = translation;
            var dictionary = GetAllControls(this);
            foreach (var form in this.OwnedForms)
            {
                var dic2 = GetAllControls(form);
                dic2.ToList().ForEach(x => dictionary.Add(x.Key, x.Value));
            }
            foreach (KeyValue kv in translation.MenuItems) if (dictionary.ContainsKey(kv.Key))
                {
                    var text = kv.Value;
                    var control = dictionary[kv.Key];
                    if (control is ComboBox)
                    {
                        var comboBox = control as ComboBox;
                        var save = comboBox.SelectedIndex;
                        comboBox.Items.Clear();
                        comboBox.Items.AddRange(text.Split(';'));
                        comboBox.SelectedIndex = save;
                    }
                    else if (control is Control)
                    {
                        (control as Control).Text = text;
                    }
                    else if (control is ToolStripMenuItem)
                    {
                        (control as ToolStripMenuItem).Text = text;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (!kv.Key.Contains("Dynamic")) Status($"{kv.Key} not on form.");
        }

        /// <summary>
        /// This methode returns a list of all controls on a form.
        /// Each entry contains a keyValue pair, where key=control.Name, value=control.Text property.
        /// If control contains more text properties (i.e. combobox) they are separated by a ;
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private List<KeyValue> GetFormControls(Form form)
        {
            var list = new List<KeyValue>();
            foreach (var kv in GetAllControls(form))
            {
                var control = kv.Value;
                var text = "no text";
                if (control is ToolStripMenuItem) text = (control as ToolStripMenuItem).Text;
                else if (control is ComboBox) text = String.Join(";", (control as ComboBox).Items.Cast<string>());
                else if (control is Control) text = (control as Control).Text;
                list.Add(new KeyValue(kv.Key, text));
            }
            return list;
        }

        /// <summary>
        /// This methode returns a dictionary (control name & Control instance) of all controls on a form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetAllControls(Form form)
        {
            var menuItems = new Dictionary<string, object>();
            menuItems.Add(form.Name, form);

            foreach (var c in GetAll(form)) menuItems.Add(c.Name, c);
            foreach (MenuStrip m in GetAll(form, typeof(MenuStrip)))
                foreach (ToolStripMenuItem item in m.Items)
                {
                    menuItems.Add(item.Name, item);
                    foreach (ToolStripMenuItem c in item.DropDownItems)
                        menuItems.Add(c.Name, c);
                }
            return menuItems;
        }
        #endregion

    }
}
