using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooseShared;
using SamEngine;
using static GooseShared.API;
using static GooseShared.API.TaskDatabaseQueryFunctions;

namespace DefaultMod
{
    public class ModEntryPoint : IMod
    {
        public SolidBrush brushBlack, brushWhite, brushGreen;
        Font consolas;

        String[] tasks;

        int InfolnNum;
        int ConslnNum;
        StringWriter cnslStream;



        void IMod.Init()
        {
            cnslStream = new StringWriter();
            Console.SetOut(cnslStream);

            tasks = API.TaskDatabase.getAllLoadedTaskIDs();
            brushBlack = new SolidBrush(Color.FromArgb(255/* /2 */, Color.Black));
            brushGreen = new SolidBrush(ColorTranslator.FromHtml("#4AF626"));

            InjectionPoints.PostRenderEvent += PostRender;
        }

        public void PostRender(GooseEntity g, Graphics graph)
        {
            InfolnNum = 0;
            ConslnNum = 0;

            drawTask(g, graph);
            drawXYpos(g, graph);

            drawConsole(g, graph);
        }

        public void InfoLine(GooseEntity g, Graphics graph, String txt)
        {
            Size txtLen = graph.MeasureString(txt, SystemFonts.DefaultFont).ToSize();
            PointF txtPos = new PointF(g.position.x + 25, g.position.y + 25 + 14 * InfolnNum);

            graph.FillRectangle(brushBlack, new Rectangle(Point.Round(txtPos), txtLen));
            graph.DrawString(txt, SystemFonts.DefaultFont, brushGreen, txtPos);

            InfolnNum++;
        }
        public void ConsLine(GooseEntity g, Graphics graph, String txt)
        {
            Size txtLen = graph.MeasureString(txt, SystemFonts.DefaultFont).ToSize();
            Size refLen = graph.MeasureString("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", SystemFonts.DefaultFont).ToSize();
            if (txtLen.Width > refLen.Width)
            {
                refLen.Width = txtLen.Width;
            }

            PointF txtPos = new PointF(g.position.x + 25, g.position.y + 25 + 14 * InfolnNum + 13*ConslnNum);

            graph.FillRectangle(brushBlack, new Rectangle(Point.Round(txtPos), refLen));
            graph.DrawString(txt, SystemFonts.DefaultFont, brushGreen, txtPos);

            ConslnNum++;
        }

        public void drawConsole(GooseEntity g, Graphics graph)
        {

            String[] consoleLines = cnslStream.ToString().Split(new[] {'\r','\n'});

            if (!(consoleLines.Length <= 0))
            {
                for (int i = consoleLines.Length - 21; i < consoleLines.Length - 1; i += 2)
                {
                    if (!(i <= -1))
                    {
                        ConsLine(g, graph, consoleLines[i]);
                    }
                }
            }
        }

        public void drawXYpos(GooseEntity g, Graphics graph)
        {
            Vector2 gPos = g.position;
            String gp = "xy: (" + Math.Round(gPos.x) + ", " + Math.Round(gPos.y) + ")";
            InfoLine(g, graph, gp);
        }
        public void drawTask(GooseEntity g, Graphics graph)
        {
            InfoLine(g, graph, tasks[g.currentTask]);
        }
    }
}