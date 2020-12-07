using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormОжидание : Form
    {

        //Bitmap animatedImage = new Bitmap(@"D:\Проекты\ControlDantist\ControlDantist\time12.gif");
        //bool currentlyAnimating = false;

        //public void AnimateImage()
        //{
        //    if (!currentlyAnimating)
        //    {
        //        //Begin the animation only once.
        //        ImageAnimator.Animate(animatedImage, new EventHandler(this.OnFrameChanged));
        //        currentlyAnimating = true;
        //    }
        //}

        //private void OnFrameChanged(object o, EventArgs e)
        //{
        //    //Force a call to the Paint event handler.
        //    this.Invalidate();
        //}
 

        public FormОжидание()
        {
            InitializeComponent();
            this.timer1.Interval = 1000;

            this.MaximizeBox = true;
            this.MinimizeBox = true;
        }

        //private void FormОжидание_Paint(object sender, PaintEventArgs e)
        //{
        //    AnimateImage();
        //    //Get the next frame ready for rendering.
        //    ImageAnimator.UpdateFrames();
        //    //Draw the next frame in the animation.
        //    e.Graphics.DrawImage(this.animatedImage, new Point(0, 0));
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.Refresh();

            //this.progressBar1.Value += 15;
            //this.timer1.Enabled = true;
            if (this.timer1.Interval == 1000)
            {
                this.label1.Text = "Проверяю .";
                this.Refresh();
            }

            if (this.timer1.Interval == 2000)
            {
                this.label1.Text = "Проверяю ..";
                this.Refresh();
            }

            if (this.timer1.Interval == 3000)
            {
                this.label1.Text = "Проверяю ...";
                this.Refresh();
            }
        }

        private void FormОжидание_Shown(object sender, EventArgs e)
        {
            //this.progressBar1.Value += 15;
            //this.timer1.Enabled = true;
            //if (this.timer1.Interval == 1000)
            //{
            //    this.label1.Text = "Проверяю .";
            //    this.Refresh();
            //}

            //if (this.timer1.Interval == 2000)
            //{
            //    this.label1.Text = "Проверяю ..";
            //    this.Refresh();
            //}

            //if (this.timer1.Interval == 3000)
            //{
            //    this.label1.Text = "Проверяю ...";
            //    this.Refresh();
            //}
        }
    }
}