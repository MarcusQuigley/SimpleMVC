using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectBilling.DataAccess;

namespace ProjectBilling.MVP
{
    public interface IView
    {
        int NONE_SELECTED { get; }
        int SelectedProjectID { get; }
        void UpdateProject(Project project);
        void LoadProjects(List<Project> projects);
        void UpdateDetails(Project project);
        void EnableControls(bool isEnabled);
        void SetEstimateColor(Color color);
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        event EventHandler<ProjectEventArgs> DetailsUpdated;
        event EventHandler SelectionChanged;
    }
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window, IView
    {
        int selectedProjectID;

        public int NONE_SELECTED
        {
            get { return -1; }
        }

        public int SelectedProjectID
        {
            get { return selectedProjectID; }
        }
        public View()
        {
            InitializeComponent();
            selectedProjectID = NONE_SELECTED;
        }
 
        public void UpdateProject(Project project)
        {
            // Null checks excluded
            IEnumerable<Project> projects =
                cboProjects.ItemsSource as
                    IEnumerable<Project>;
            Project projectToUpdate =
                projects.Where(p => p.ID == project.ID)
                    .First() as Project;
            projectToUpdate.Name = project.Name;
            projectToUpdate.Estimate = project.Estimate;
            projectToUpdate.Actual = project.Actual;
            if (project.ID == SelectedProjectID)
                UpdateDetails(project);
        }

        public void LoadProjects(List<Project> projects)
        {
            cboProjects.ItemsSource = projects;
            cboProjects.DisplayMemberPath = "Name";
            cboProjects.SelectedValuePath = "ID";
        }

        public void UpdateDetails(Project project)
        {
            txtActual.Text = project.Actual.ToString();
            txtEstimate.Text = project.Estimate.ToString();

            DetailsUpdated(this, new ProjectEventArgs(project));
        }

        public void EnableControls(bool isEnabled)
        {
            txtActual.IsEnabled = isEnabled;
            txtEstimate.IsEnabled = isEnabled;
            btnUpdate.IsEnabled = isEnabled;
        }

        public void SetEstimateColor(Color color)
        {
            txtEstimate.Foreground = new SolidColorBrush(color);
        }

        public event EventHandler<ProjectEventArgs> ProjectUpdated = delegate { };

        public event EventHandler<ProjectEventArgs> DetailsUpdated = delegate { };

        public event EventHandler SelectionChanged = delegate { };

        private void cboProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProjectID = cboProjects.SelectedIndex;
            SelectionChanged(this, new EventArgs());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Project p = new Project()
            {
                Name = cboProjects.Text,
                ID = cboProjects.SelectedIndex,
                Actual = double.Parse(txtActual.Text),
                Estimate = double.Parse(txtEstimate.Text)

            };
            ProjectUpdated(this, new ProjectEventArgs(p));
        }
    }
}
