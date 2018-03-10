using System;
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
		public Form1()
		{
			InitializeComponent();
			comboBox1.SelectedIndex = 1;
		}

		public TransLation translation;

		public bool InProcess { get; private set; }

		private void btnLoad_Click(object sender, EventArgs e)
		{
			translation = TransLation.Load(comboBox1.Text);
			if (translation.MenuItems != null)
				distribute(translation);
		}

		private void distribute(TransLation translation)
		{
			Dictionary<string, object> dictionary = GetAllControls(this);
			foreach (KeyValue kv in translation.MenuItems)
			{
				var text = kv.Value;
				var c = dictionary[kv.Key];
				if (c is ComboBox)
				{
					var comboBox = c as ComboBox;
					var subtext = text.Split(';');
					var idx = comboBox.SelectedIndex;
					comboBox.Items.Clear();
					comboBox.Items.AddRange(subtext);
					comboBox.SelectedIndex = idx;
				}
				else if (c is Control)
				{
					(c as Control).Text = text;
				}
				else if (c is ToolStripMenuItem)
				{
					(c as ToolStripMenuItem).Text = text;
				}
				else
				{
					continue;
				}

			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			translation.MenuItems = GetFormControls(this);
			translation.Save(comboBox1.Text);
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
			list.Add( new KeyValue(form.Name, form.Text) );

			foreach (var c in GetAll(form))
			{
				var text = c.Text;
				if (c is ComboBox)
				{
					text = "";
					foreach (string item in (c as ComboBox).Items) text += $";{item}";
					text = text.TrimStart(';');
				}
				list.Add(new KeyValue(c.Name, text));
			}
			foreach (MenuStrip menu in GetAll(form, typeof(MenuStrip)))
				foreach (ToolStripMenuItem item in menu.Items)
				{
					list.Add(new KeyValue(item.Name, item.Text));
					foreach (ToolStripMenuItem down in item.DropDownItems)
						list.Add(new KeyValue(down.Name, down.Text));
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

			foreach (var c in GetAll(form))menuItems.Add(c.Name, c);
			foreach (MenuStrip m in GetAll(form, typeof(MenuStrip)))
				foreach (ToolStripMenuItem item in m.Items)
				{
					menuItems.Add(item.Name, item);
					foreach (ToolStripMenuItem c in item.DropDownItems)
						menuItems.Add(c.Name, c);
				}
			return menuItems;
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
	}
}
