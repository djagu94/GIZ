using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace GIZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] board;
        Node[] countries;

        int W, K, CNT, fromW, fromK, toW, toK;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                readFile();

                int put = -1;
                for (int i = 0; i < W; i++)
                    for (int j = 0; j < K; j++)
                        if (board[i, j] < 0)
                            floodFill(i, j, board[i, j], ++put);

                CNT = ++put;
                initializeCountries();

                var dist = dijkstra(board[fromW, fromK], board[toW, toK]);
                var route = getRoute();
                saveFile(dist, route);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void floodFill(int w, int k, int search, int put)
        {
            if (w >= W || k >= K || w < 0 || k < 0)
                return;
            if (board[w, k] == search)
            {
                board[w, k] = put;
                floodFill(w + 1, k, search, put);
                floodFill(w - 1, k, search, put);
                floodFill(w, k + 1, search, put);
                floodFill(w, k - 1, search, put);
            }
        }

        private void initializeCountries()
        {
            countries = new Node[CNT];

            for (int i = 0; i < CNT; i++)
                countries[i] = new Node(i, CNT);

            for (int i = 0; i < W; i++)
                for (int j = 0; j < K; j++)
                {
                    countries[board[i, j]].Count++;
                    countries[board[i, j]].setField(i, j);
                }

            for (int i = 0; i < W; i++)
                for (int j = 0; j < K; j++)
                    neighbourFinder(i, j);
        }

        private void neighbourFinder(int w, int k)
        {
            if (w + 1 < W && board[w, k] != board[w + 1, k] && countries[board[w, k]].EdgesFrom[board[w + 1, k]] == null)
            {
                countries[board[w, k]].EdgesFrom[board[w + 1, k]] = new Edge(countries[board[w, k]], countries[board[w + 1, k]]);
                countries[board[w + 1, k]].EdgesFrom[board[w, k]] = new Edge(countries[board[w + 1, k]], countries[board[w, k]]);
            }

            if (k + 1 < K && board[w, k] != board[w, k + 1] && countries[board[w, k]].EdgesFrom[board[w, k + 1]] == null)
            {
                countries[board[w, k]].EdgesFrom[board[w, k + 1]] = new Edge(countries[board[w, k]], countries[board[w, k + 1]]);
                countries[board[w, k + 1]].EdgesFrom[board[w, k]] = new Edge(countries[board[w, k + 1]], countries[board[w, k]]);
            }
        }

        private long dijkstra(int from, int to)
        {
            for (int i = 0; i < CNT; i++)
                countries[i].Dist = int.MaxValue;
            countries[from].Dist = 0;

            var Q = new PriorityQueue(countries);
            while (!Q.IsEmpty)
            {
                Node u = Q.Pop();
                for (int i = 0; i < CNT; i++)
                    if (u.EdgesFrom[i] != null)
                    {
                        long newDist = u.Dist + u.EdgesFrom[i].cost;
                        if (newDist < u.EdgesFrom[i].to.Dist)
                        {
                            u.EdgesFrom[i].to.Dist = newDist;
                            u.EdgesFrom[i].to.Parent = u;
                        }
                    }
            }

            return countries[to].Dist;
        }

        private Stack<Node> getRoute()
        {
            Stack<Node> route = new Stack<Node>();
            Node curr = countries[board[toW, toK]];
            while (curr != null)
            {
                route.Push(curr);
                curr = curr.Parent;
            }

            return route;
        }

        private void readFile()
        {
            string[] txt;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
                throw new Exception("Invalid file");
            txt = File.ReadAllLines(openFileDialog.FileName);

            //board size
            var size = Regex.Split(txt[0].Trim(), " ");
            W = int.Parse(size[0]);
            K = int.Parse(size[1]);
            board = new int[W, K];


            for (int i = 0; i < W; i++)
            {
                var line = Regex.Split(txt[i + 1].Trim(), " ");
                for (int j = 0; j < K; j++)
                    board[i, j] = int.Parse(line[j]) - 2;
            }            

            var from = Regex.Split(txt[txt.Length - 2].Trim(), " ");
            fromW = int.Parse(from[0]);
            fromK = int.Parse(from[1]);

            var to = Regex.Split(txt[txt.Length - 1].Trim(), " ");
            toW = int.Parse(to[0]);
            toK = int.Parse(to[1]);
        }

        private void saveFile(long dist, Stack<Node> route)
        {
            Node curr;
            SaveFileDialog dialog = new SaveFileDialog { Filter = "Text Files(*.txt)|*.txt|All(*.*)|*" };
            if (dialog.ShowDialog() == true)
            {
                var output = new StringBuilder();
                output.AppendLine(dist.ToString());
                while (route.Count > 0)
                {
                    curr = route.Pop();
                    output.AppendLine(curr.ToString());
                }

                if (File.Exists(dialog.FileName))
                    File.Delete(dialog.FileName);
                File.WriteAllText(dialog.FileName, output.ToString(), Encoding.UTF8);
                System.Diagnostics.Process.Start(dialog.FileName);
            }
        }
    }
}
