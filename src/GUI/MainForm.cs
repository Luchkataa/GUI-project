using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

            viewPort.MouseWheel += ViewPortMouseWheel;
            viewPort.Focus();
            viewPort.TabStop = true;

        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

        void DrawStarSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomStar();

            statusBar.Items[0].Text = "Последно действие: Рисуване на звезда";

            viewPort.Invalidate();
        }

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (pickUpSpeedButton.Checked)
            {
                Shape clickedShape = dialogProcessor.ContainsPoint(e.Location);
                if (clickedShape != null)
                {
                    if (ModifierKeys.HasFlag(Keys.Control))
                    {
                        if (dialogProcessor.Selection.Contains(clickedShape))
                            dialogProcessor.Selection.Remove(clickedShape);
                        else
                            dialogProcessor.Selection.Add(clickedShape);
                    }
                    else
                    {
                        dialogProcessor.Selection.Clear();
                        dialogProcessor.Selection.Add(clickedShape);
                    }

                    statusBar.Items[0].Text = "Последно действие: Селекция";
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                    viewPort.Invalidate();
                }
            }

        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режм на "влачене", то избрания елемент се транслира.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        /// <summary>
		/// Мащабиране на примитив чрез мишката.
		/// Излизаме от режим мащабиране.
		/// </summary>
        private void ViewPortMouseWheel(object sender, MouseEventArgs e)
        {
            if (dialogProcessor.Selection.Count > 0)
            {
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    float scaleFactor = e.Delta > 0 ? 1.1f : 0.9f;

                    var center = GetSelectionCenter();
                    dialogProcessor.ScaleAt(scaleFactor, scaleFactor, center);

                    statusBar.Items[0].Text = "Последно действие: Мащабиране с мишката";
                }
                else if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    float angle = e.Delta > 0 ? 10f : -10f;
                    dialogProcessor.Rotate(angle);
                    statusBar.Items[0].Text = "Последно действие: Завъртане с мишката";
                }

                viewPort.Invalidate();
            }
        }

        private PointF GetSelectionCenter()
        {
            if (dialogProcessor.Selection.Count == 0)
                return PointF.Empty;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var shape in dialogProcessor.Selection)
            {
                var bounds = shape.GetTransformedBounds();
                if (bounds.Left < minX) minX = bounds.Left;
                if (bounds.Top < minY) minY = bounds.Top;
                if (bounds.Right > maxX) maxX = bounds.Right;
                if (bounds.Bottom > maxY) maxY = bounds.Bottom;
            }

            return new PointF((minX + maxX) / 2, (minY + maxY) / 2);
        }


        private void Ellipse_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void Triangle_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

            viewPort.Invalidate();
        }

        private void Trapezoid_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTrapezoid();

            statusBar.Items[0].Text = "Последно действие: Рисуване на трапец";

            viewPort.Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) //Color Picker
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK && dialogProcessor.Selection.Count > 0)
            {
                foreach (var shape in dialogProcessor.Selection)
                {
                    shape.BorderColor = colorDialog1.Color;
                }

                statusBar.Items[0].Text = "Последно действие: Промяна на цвета на фигура(и)";
                viewPort.Invalidate();
            }
        }


        private void trackBarBorderWidth_Scroll(object sender, EventArgs e)
        {
            if (dialogProcessor.Selection.Count > 0)
            {
                foreach (var shape in dialogProcessor.Selection)
                {
                    shape.BorderWidth = trackBarBorderWidth.Value;
                }

                viewPort.Invalidate();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) //rotate kopche
        {
            if (dialogProcessor.Selection != null)
            {
                dialogProcessor.Rotate(15); // Завърта избрания елемент с 15 градуса
                viewPort.Invalidate(); // Прерисува екрана
            }
            else
            {
                MessageBox.Show("Няма избран елемент за завъртане.");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dialogProcessor.Selection != null)
            {
                dialogProcessor.DeleteSelected();
                statusBar.Items[0].Text = "Последно действие: Изтриване на примитив";
                viewPort.Invalidate();
            }
            else
            {
                MessageBox.Show("Няма избран примитив за изтриване.");
            }
        }

        private void GroupShape_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupSelectedShapes();
            viewPort.Invalidate();
        }

        private void UngroupShape_Click(object sender, EventArgs e)
        {
            dialogProcessor.UngroupSelectedShape();
            viewPort.Invalidate();
        }
    }
}
