namespace Pretorianie.Tytan.Forms
{
    public partial class RegistryAddFavorite : Core.BaseForms.BasePackageForm
    {
        public RegistryAddFavorite()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the initial state of the GUI.
        /// </summary>
        public void Initialize(string location)
        {
            ActiveControl = txtName;

            txtName.Text = location;
            txtLocation.Text = location;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of favorite item.
        /// </summary>
        public string FavoriteName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        /// <summary>
        /// Gets or sets the location inside registry pointed by favorite item.
        /// </summary>
        public string FavoriteLocation
        {
            get { return txtLocation.Text; }
            set { txtLocation.Text = value; }
        }

        #endregion
    }
}

