using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace STL_ACT_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BatchProcessing batchs;
        private string nomProgra;
        private string operacion;
        private string idProceso;
        private int tme;
        private int globTime;
        private int TAM_LOTE;

        public MainWindow()
        {
            batchs = new BatchProcessing();
            globTime = 0;
            TAM_LOTE = 5;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            batchs.totalProcess = (int)txtBoxNumProces.Value;

            if (batchs.totalProcess > 0) {
                ToggleFields(true); // WINDOW : Habilitar campos
                batchs.idx = 1;
                /* --- Window --- */
                lblNumLot.Content = batchs.idx;
                tblProFi.Items.Clear();
            }
        }

        private void bttnNxtPro_Click(object sender, RoutedEventArgs e)
        {
            nomProgra = txtBoxNomPro.Text;
            operacion = txtBoxOpeIzq.Text + cbOperators.Text + txtBoxOpeDer.Text;
            idProceso = txtBoxIdPro.Text;

            if (ValidateFields()) {
                /* --- Add batch --- */
                if (--TAM_LOTE == 0) {
                    TAM_LOTE = 5;
                    batchs.idx++;

                    lblNumLot.Content = batchs.idx.ToString(); // WINDOW : Update label batch idx
                }

                string resultado = SolveOperation((int)txtBoxOpeIzq.Value, cbOperators.Text, (int)txtBoxOpeDer.Value);

                /* --- Add process to current batch --- */
                Process p = new Process(nomProgra, operacion, tme, idProceso, resultado);
                batchs.processes.Enqueue(p);

                /* --- WINDOW --- */
                //txtBoxNomPro.Clear();
                txtBoxOpeIzq.Value = 0;
                txtBoxOpeDer.Value = 0;
                txtBoxTME.Value = 1;
                //txtBoxIdPro.Clear();

                batchs.totalProcess--;
                if (batchs.totalProcess == 0) {
                    // MessageBox.Show("Fin de registro.", "Info");
                    StartProcessing();
                }
                txtBoxNumProces.Text = batchs.totalProcess.ToString();
            }
        }

        private bool ValidateFields()
        {
            if (nomProgra.Length == 0 || operacion.Length == 0 || txtBoxTME.Text.Length == 0 || idProceso.Length == 0) {
                MessageBox.Show("Algun campo vacio.", "Error");
                return false;
            }
            Int32.TryParse(txtBoxTME.Text, out tme);
            if (tme <= 0) {
                MessageBox.Show("El Tiempo Estimado Maximo debe ser mayor a cero.", "Error");
                return false;
            }
            if (cbOperators.Text == "/" && txtBoxOpeDer.Value == 0) {
                MessageBox.Show("Operacion invalida.", "Error");
                return false;
            }
            Process p = new Process { ProcessId = idProceso };
            if (batchs.processes.Contains(p)) {
                MessageBox.Show("Id repetido.", "Error");
                return false;
            }
            return true;
        }

        private string SolveOperation(int num1, string ope, int num2)
        {
            double res = 0;
            switch (ope) {
                case "+": res = num1 + num2; break;
                case "-": res = num1 - num2; break;
                case "*": res = num1 * num2; break;
                case "/": res = num1 / num2; break;
            }
            return res.ToString();
        }

        private async void StartProcessing()
        {
            Process p;
            int remainingBatchs = batchs.idx;

            ToggleFields(false); // WINDOW

            while (remainingBatchs > 0) {
                lblLotPen.Content = remainingBatchs--.ToString(); // WINDOW

                /* ----- LOTE EN EJECUCION ----- */
                batchs.MoveProsToCurrBatch();
                AddToCurrBacthTbl();

                for (int i = 0; i < 5; i++) {
                    if (batchs.prosCurrBatch.Count != 0) {
                        p = batchs.prosCurrBatch.Dequeue();

                        /* ----- PROCESO EN EJECUCION ----- */
                        lblNomPro_PE.Content = p.PrgmmrName;
                        lblTME_PE.Content = p.TME;
                        lblOpe_PE.Content = p.Operation;
                        lblNumPro.Content = p.ProcessId;

                        await DoProcess(p.TME);

                        /* ----- PROCESOS TERMINADOS ----- */
                        tblLotEje.Items.Remove(p);
                        tblProFi.Items.Add(p);
                    }
                }
                tblProFi.Items.Add(new Process());
            }
            ToggleFields(false);
            lblLotPen.Content = "0";
            MessageBox.Show("Fin de procesos.", "Info");
        }

        private void AddToCurrBacthTbl()
        {
            foreach (Process p in batchs.prosCurrBatch) {
                tblLotEje.Items.Add(p);
            }
        }

        private async Task DoProcess(int TME)
        {
            int remainingTime = TME;
            int elapsedTime = 0;
            while (remainingTime > 0) {
                lblTieTra.Content = (elapsedTime++).ToString() + " sec";
                lblTieRes.Content = (remainingTime--).ToString() + " sec";
                lblContGlo.Content = (globTime++).ToString();
                /* Espera un segundo */
                await Task.Delay(1000);
            }
        }

        private void ToggleFields(bool state)
        {
            txtBoxNomPro.IsEnabled = state;
            txtBoxOpeIzq.IsEnabled = state;
            txtBoxOpeDer.IsEnabled = state;
            cbOperators.IsEnabled = state;
            txtBoxTME.IsEnabled = state;
            txtBoxIdPro.IsEnabled = state;
            bttnNxtPro.IsEnabled = state;

            txtBoxNumProces.IsEnabled = !state;
            bttnStart.IsEnabled = !state;
        }
    }
}
