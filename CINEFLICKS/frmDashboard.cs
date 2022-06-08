using Bunifu.UI.WinForms.BunifuAnimatorNS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CINEFLICKS
{
    public partial class frmDashboard : Form
    {
        clsDBCon DBCon = new clsDBCon(); // Class object - clsDBcon.cs
        MySqlDataReader dr; // MySqlDataReader object

        clsSession objSession = new clsSession(); // Class object - clsSession.cs

        clsProperties.ManageMovies objData = new clsProperties.ManageMovies(); // Class object - Data

        DateTime backDate = DateTime.Now.AddDays(-30); // Going back 30 days from today
        string back30Date = string.Empty;

        DateTime today = DateTime.Now; // Today date
        string todayDate = string.Empty;

        public frmDashboard()
        {
            InitializeComponent();
        }
        
        private async void frmDashboard_Load(object sender, EventArgs e)
        {
            // Get current DateTime - DateTime object
            DateTime aDate = DateTime.Now;
            lblDate.Text = aDate.ToString("MMMM dd");

            tmrTime.Start();

            string uName = objSession.GetName();
            if (uName != "")
            {
                btnLogState.Text = "Log Out";
            }
            else
            {
                btnLogState.Text = "Log In";
            }

            // Disable buttons for guest users
            if (uName == "")
            {
                btnManageMov.Enabled = false;
                btnManageActors.Enabled = false;
                btnManageGenre.Enabled = false;
            }

            // Disable manage user button
            if (uName != "admin")
            {
                btnManageUsr.Enabled = false;
            }

            // Movie and TV show count
            showMovCount();

            // Bunifu card transitions
            await Task.Delay(500);
            bunifuTransition1.ShowSync(bunifuCards1, false, Animation.Mosaic);
            bunifuTransition1.ShowSync(bunifuCards2, false, Animation.Mosaic);

            // Fetch and load new movies to DGV
            LoadNewMovToGridView();

            // Fetch and load upcoming movies to DGV
            LoadUpcomMovToGridView();

            // Fetch and load top rated movies to DGV
            LoadTopMovToGridView();
        }

        // Movie and TV show count
        private void showMovCount()
        {
            try
            {
                DBCon.OpenConection(); // Calling the method to open the DB connection
                string checkQry = "SELECT COUNT(mov_name) FROM cineflicksdb.tbl_movie WHERE mov_type = 'Movie';";
                int mCount = DBCon.ExecuteScalar(checkQry);

                lblMovCount.Text = mCount.ToString();

                checkQry = "SELECT COUNT(mov_name) FROM cineflicksdb.tbl_movie WHERE mov_type = 'TV-Show';";
                int tvCount = DBCon.ExecuteScalar(checkQry);

                lblTVCount.Text = tvCount.ToString();
            }
            catch (MySqlException ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            finally
            {
                DBCon.CloseConnection(); // Calling the method to close the DB connection
            }
        }

        // Function - LoadNewMovToGridView
        private void LoadNewMovToGridView()
        {
            string back30Date = backDate.ToString("yyyy-MM-dd");
            string todayDate = today.ToString("yyyy-MM-dd");

            try
            {
                DBCon.OpenConection(); // Calling the method to open the DB connection
                dgvNewMov.DataSource = DBCon.ShowDataInGridView("SELECT mov_name AS 'Name', mov_type AS 'Type', mov_rating AS 'Rating', mov_prod_company AS 'Prod. Company', mov_release_date AS 'Release Date', mov_duration AS 'Duration', mov_language AS Language FROM tbl_movie WHERE mov_release_date >= '" + back30Date + "' AND mov_release_date <= '" + todayDate + "'");
            }
            catch (MySqlException ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            finally
            {
                DBCon.CloseConnection(); // Calling the method to close the DB connection
            }
        }

        // Function - LoadUpcomMovToGridView
        private void LoadUpcomMovToGridView()
        {
            string todayDate = today.ToString("yyyy-MM-dd");

            try
            {
                DBCon.OpenConection(); // Calling the method to open the DB connection
                dgvUpcomMov.DataSource = DBCon.ShowDataInGridView("SELECT mov_name AS 'Name', mov_type AS 'Type', mov_rating AS 'Rating', mov_prod_company AS 'Prod. Company', mov_release_date AS 'Release Date', mov_duration AS 'Duration', mov_language AS Language FROM tbl_movie WHERE mov_release_date > '" + todayDate + "'");
            }
            catch (MySqlException ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            finally
            {
                DBCon.CloseConnection(); // Calling the method to close the DB connection
            }
        }

        // Function - LoadTopMovToGridView
        private void LoadTopMovToGridView()
        {
            try
            {
                DBCon.OpenConection(); // Calling the method to open the DB connection
                dgvTopMov.DataSource = DBCon.ShowDataInGridView("SELECT mov_name AS 'Name', mov_type AS 'Type', mov_rating AS 'Rating', mov_prod_company AS 'Prod. Company', mov_release_date AS 'Release Date', mov_duration AS 'Duration', mov_language AS Language FROM tbl_movie WHERE mov_rating >= 8");
            }
            catch (MySqlException ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }
            finally
            {
                DBCon.CloseConnection(); // Calling the method to close the DB connection
            }
        }

        // Button - Login and Logout
        private void btnLogState_Click(object sender, EventArgs e)
        {
            if (btnLogState.Text == "Log In")
            {
                frmLogin frmLogin = new frmLogin();
                frmLogin.Show();
                this.Hide();
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    frmUserSelection frmUserSelection = new frmUserSelection();
                    frmUserSelection.Show();
                    this.Hide();

                    objSession.SetName("");
                }
            }
        }

        private void panelMain_Click(object sender, EventArgs e)
        {

        }

        // Button - About
        private void btnAbout_Click(object sender, EventArgs e)
        {
            Form formBackground = new Form();
            try
            {
                using (frmAbout uu = new frmAbout())
                {
                    formBackground.StartPosition = FormStartPosition.Manual;
                    formBackground.FormBorderStyle = FormBorderStyle.None;
                    formBackground.Opacity = .50d;
                    formBackground.BackColor = Color.Black;
                    formBackground.WindowState = FormWindowState.Maximized;
                    formBackground.TopMost = true;
                    formBackground.Location = this.Location;
                    formBackground.ShowInTaskbar = false;
                    formBackground.Show();

                    uu.Owner = formBackground;
                    uu.ShowDialog();

                    formBackground.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                formBackground.Dispose();
            }
        }

        // Show time
        private void tmrTime_Tick(object sender, EventArgs e)
        {
            DateTime atime = DateTime.Now;
            this.lblTime.Text = atime.ToString("hh:mm tt");
        }

        // Button - Manage Users
        private void btnManageUsr_Click(object sender, EventArgs e)
        {
            frmManageUsers frmManageUsers = new frmManageUsers();
            frmManageUsers.Show();
            this.Hide();
        }

        // Button - Manage Genres
        private void btnManageGenre_Click(object sender, EventArgs e)
        {
            frmManageGenres frmManageGenres = new frmManageGenres();
            frmManageGenres.Show();
            this.Hide();
        }

        // Button - Manage Actors
        private void btnManageActors_Click(object sender, EventArgs e)
        {
            frmManageActors frmManageActors = new frmManageActors();
            frmManageActors.Show();
            this.Hide();
        }

        // Button - Manage Movies
        private void btnManageMov_Click(object sender, EventArgs e)
        {
            frmManageMovies frmManageMovies = new frmManageMovies();
            frmManageMovies.Show();
            this.Hide();
        }

        // Button - Search Movies
        private void btnSearchMov_Click(object sender, EventArgs e)
        {
            frmSearchMovies frmSearchMovies = new frmSearchMovies();
            frmSearchMovies.Show();
            this.Hide();
        }

        // Form Closing
        private void frmDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Page navigation 
        private void btnNewMov_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageName = "pgNewMov";
        }

        // Page navigation 
        private void btnUpcomMov_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageName = "pgUpcomMov";
        }

        // Page navigation 
        private void btnTopMov_Click(object sender, EventArgs e)
        {
            bunifuPages1.PageName = "pgTopMov";
        }

        // New Movie PopUp funtion
        private void dgvNewMov_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string tempMovName = dgvNewMov.Rows[e.RowIndex].Cells[0].Value.ToString();
                int tempMovInRating = (int)dgvNewMov.Rows[e.RowIndex].Cells[2].Value;
                string tempMovPrCompany = dgvNewMov.Rows[e.RowIndex].Cells[3].Value.ToString();

                try
                {
                    DBCon.OpenConection(); // Calling the method to open the DB connection
                    dr = DBCon.DataReader("SELECT mov_id FROM tbl_movie WHERE mov_name = '" + tempMovName + "' AND mov_rating = '" + tempMovInRating + "' AND mov_prod_company = '" + tempMovPrCompany + "'");

                    if (dr.Read())
                    {
                        objData.MovID = (dr["mov_id"].ToString());

                        Form formBackground = new Form();
                        try
                        {
                            using (frmViewMovie uu = new frmViewMovie())
                            {
                                formBackground.StartPosition = FormStartPosition.Manual;
                                formBackground.FormBorderStyle = FormBorderStyle.None;
                                formBackground.Opacity = .50d;
                                formBackground.BackColor = Color.Black;
                                formBackground.WindowState = FormWindowState.Maximized;
                                formBackground.TopMost = true;
                                formBackground.Location = this.Location;
                                formBackground.ShowInTaskbar = false;
                                formBackground.Show();

                                uu.Owner = formBackground;
                                uu.ShowDialog();

                                formBackground.Dispose();
                            }

                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            string title = "Error";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            formBackground.Dispose();
                        }

                    }
                    else
                    {
                        string message = "The movie does not exist";
                        string title = "Warning";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    }
                }
                catch (MySqlException ex)
                {
                    string message = ex.Message;
                    string title = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                }
                finally
                {
                    DBCon.CloseConnection(); // Calling the method to close the DB connection
                }
            }
        }

        // Upcoming Movie PopUp funtion
        private void dgvUpcomMov_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string tempMovName = dgvUpcomMov.Rows[e.RowIndex].Cells[0].Value.ToString();
                int tempMovInRating = (int)dgvUpcomMov.Rows[e.RowIndex].Cells[2].Value;
                string tempMovPrCompany = dgvUpcomMov.Rows[e.RowIndex].Cells[3].Value.ToString();

                try
                {
                    DBCon.OpenConection(); // Calling the method to open the DB connection
                    dr = DBCon.DataReader("SELECT mov_id FROM tbl_movie WHERE mov_name = '" + tempMovName + "' AND mov_rating = '" + tempMovInRating + "' AND mov_prod_company = '" + tempMovPrCompany + "'");

                    if (dr.Read())
                    {
                        objData.MovID = (dr["mov_id"].ToString());

                        Form formBackground = new Form();
                        try
                        {
                            using (frmViewMovie uu = new frmViewMovie())
                            {
                                formBackground.StartPosition = FormStartPosition.Manual;
                                formBackground.FormBorderStyle = FormBorderStyle.None;
                                formBackground.Opacity = .50d;
                                formBackground.BackColor = Color.Black;
                                formBackground.WindowState = FormWindowState.Maximized;
                                formBackground.TopMost = true;
                                formBackground.Location = this.Location;
                                formBackground.ShowInTaskbar = false;
                                formBackground.Show();

                                uu.Owner = formBackground;
                                uu.ShowDialog();

                                formBackground.Dispose();
                            }

                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            string title = "Error";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            formBackground.Dispose();
                        }

                    }
                    else
                    {
                        string message = "The movie does not exist";
                        string title = "Warning";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    }
                }
                catch (MySqlException ex)
                {
                    string message = ex.Message;
                    string title = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                }
                finally
                {
                    DBCon.CloseConnection(); // Calling the method to close the DB connection
                }
            }
        }

        // Top-Rated Movie PopUp funtion
        private void dgvTopMov_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string tempMovName = dgvTopMov.Rows[e.RowIndex].Cells[0].Value.ToString();
                int tempMovInRating = (int)dgvTopMov.Rows[e.RowIndex].Cells[2].Value;
                string tempMovPrCompany = dgvTopMov.Rows[e.RowIndex].Cells[3].Value.ToString();

                try
                {
                    DBCon.OpenConection(); // Calling the method to open the DB connection
                    dr = DBCon.DataReader("SELECT mov_id FROM tbl_movie WHERE mov_name = '" + tempMovName + "' AND mov_rating = '" + tempMovInRating + "' AND mov_prod_company = '" + tempMovPrCompany + "'");

                    if (dr.Read())
                    {
                        objData.MovID = (dr["mov_id"].ToString());

                        Form formBackground = new Form();
                        try
                        {
                            using (frmViewMovie uu = new frmViewMovie())
                            {
                                formBackground.StartPosition = FormStartPosition.Manual;
                                formBackground.FormBorderStyle = FormBorderStyle.None;
                                formBackground.Opacity = .50d;
                                formBackground.BackColor = Color.Black;
                                formBackground.WindowState = FormWindowState.Maximized;
                                formBackground.TopMost = true;
                                formBackground.Location = this.Location;
                                formBackground.ShowInTaskbar = false;
                                formBackground.Show();

                                uu.Owner = formBackground;
                                uu.ShowDialog();

                                formBackground.Dispose();
                            }

                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            string title = "Error";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            formBackground.Dispose();
                        }

                    }
                    else
                    {
                        string message = "The movie does not exist";
                        string title = "Warning";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    }
                }
                catch (MySqlException ex)
                {
                    string message = ex.Message;
                    string title = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                }
                finally
                {
                    DBCon.CloseConnection(); // Calling the method to close the DB connection
                }
            }
        }
    }
}