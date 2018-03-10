using MyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			T = TransLation.Load("EN");
		}

		public TransLation T;

		private void btnLoad_Click(object sender, EventArgs e)
		{
			T = TransLation.Load(txtFilename.Text);
			comboBox1.DataSource = T.Combo;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			T.MenuItems = GetMenuControls(this);
			T.Combo = new List<string>();
			foreach (var item in comboBox1.Items)
			T.Combo.Add(item.ToString());
			T.Save(txtFilename.Text);
		}

		private List<KeyValue> GetMenuControls(Form form)
		{
			var menuItems = new List<KeyValue>();

			foreach (var c in My.GetAll(form))
				menuItems.Add(new KeyValue(c.Name, c.Text));

			foreach (MenuStrip m in My.GetAll(form, typeof(MenuStrip)))
				foreach (ToolStripMenuItem item in m.Items)
				{
					menuItems.Add(new KeyValue(item.Name, item.Text));
					foreach (ToolStripMenuItem c in item.DropDownItems)
						menuItems.Add(new KeyValue(c.Name, c.Text));
				}
			return menuItems;
		}
	}
}
