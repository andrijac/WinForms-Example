using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
	public partial class Form1 : Form
	{
		private Presenter presenter;

		public Form1()
		{
			this.InitializeComponent();

			this.presenter = new Presenter()
			{
				Name = "Foo",
				SecondName = "Bar",
				FormWidth = 572
			};
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.textBox1.DataBindings.Add("Text", this.presenter, "Name");
			this.DataBindings.Add("Width", this.presenter, "FormWidth");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.presenter.Name = "Drugo ime";
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			int width;

			if (int.TryParse(this.textBox2.Text, out width))
			{
				if (width < 209)
				{
					return;
				}

				this.presenter.FormWidth = width;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int test = this.presenter.FormWidth;
			this.presenter.FormWidth = test + 10;
		}
	}
}