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
		#region FIELDS
		public TransLation translation;
		public bool InProcess;
		private int idx;
		#endregion
		public Form1()
		{
			InitializeComponent();
			MyLib.My.InitStatus(toolStripStatusLabel1);
			translation = new TransLation();
			// Set default language to English.
			comboBox1.SelectedIndex = 0;
		}
		#region FORM EVENTS
		private void btnLoad_Click(object sender, EventArgs e) => Dispatch(TransLation.Load($"Data\\{comboBox1.Text}.xml"));

		private void btnSave_Click(object sender, EventArgs e)
		{
			translation.MenuItems = GetFormControls(this);
			translation.Save();
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
		#endregion
		#region METHODES
		private void Dispatch(TransLation translation)
		{
			var dictionary = GetAllControls(FindForm());
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
