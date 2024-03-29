
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;

	private global::Gtk.Action FiguresAction;

	private global::Gtk.Action AddEllipseAction;

	private global::Gtk.Action AddRectangleAction;

	private global::Gtk.Action AddPolyLineAction;

	private global::Gtk.Action AddSimpleTextAction;

	private global::Gtk.Action AddMultiLineTextAction;

	private global::Gtk.Action AddLineConnectionAction;

	private global::Gtk.Action EditAction;

	private global::Gtk.Action UndoAction;

	private global::Gtk.Action RedoAction;

	private global::Gtk.VBox vbox1;

	private global::Gtk.MenuBar menubar1;

	private global::MonoHotDraw.SteticComponent mhdcanvas;

	private global::Gtk.HBox hbox1;

	private global::Gtk.HBox hbox2;

	private global::Gtk.Label label2;

	private global::Gtk.HScale zoomscale;

	private global::Gtk.Label label3;

	private global::Gtk.Label visiblearea;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.FiguresAction = new global::Gtk.Action ("FiguresAction", global::Mono.Unix.Catalog.GetString ("Figures"), null, null);
		this.FiguresAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Figures");
		w1.Add (this.FiguresAction, null);
		this.AddEllipseAction = new global::Gtk.Action ("AddEllipseAction", global::Mono.Unix.Catalog.GetString ("Add Ellipse"), null, null);
		this.AddEllipseAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add Ellipse");
		w1.Add (this.AddEllipseAction, null);
		this.AddRectangleAction = new global::Gtk.Action ("AddRectangleAction", global::Mono.Unix.Catalog.GetString ("Add Rectangle"), null, null);
		this.AddRectangleAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add Rectangle");
		w1.Add (this.AddRectangleAction, null);
		this.AddPolyLineAction = new global::Gtk.Action ("AddPolyLineAction", global::Mono.Unix.Catalog.GetString ("Add PolyLine"), null, null);
		this.AddPolyLineAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add PolyLine");
		w1.Add (this.AddPolyLineAction, null);
		this.AddSimpleTextAction = new global::Gtk.Action ("AddSimpleTextAction", global::Mono.Unix.Catalog.GetString ("Add SimpleText"), null, null);
		this.AddSimpleTextAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add SimpleText");
		w1.Add (this.AddSimpleTextAction, null);
		this.AddMultiLineTextAction = new global::Gtk.Action ("AddMultiLineTextAction", global::Mono.Unix.Catalog.GetString ("Add MultiLineText"), null, null);
		this.AddMultiLineTextAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add MultiLineText");
		w1.Add (this.AddMultiLineTextAction, null);
		this.AddLineConnectionAction = new global::Gtk.Action ("AddLineConnectionAction", global::Mono.Unix.Catalog.GetString ("Add LineConnection"), null, null);
		this.AddLineConnectionAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add LineConnection");
		w1.Add (this.AddLineConnectionAction, null);
		this.EditAction = new global::Gtk.Action ("EditAction", global::Mono.Unix.Catalog.GetString ("Edit"), null, null);
		this.EditAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Edit");
		w1.Add (this.EditAction, null);
		this.UndoAction = new global::Gtk.Action ("UndoAction", global::Mono.Unix.Catalog.GetString ("_Deshacer"), null, "gtk-undo");
		this.UndoAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Deshacer");
		w1.Add (this.UndoAction, "<Control>z");
		this.RedoAction = new global::Gtk.Action ("RedoAction", global::Mono.Unix.Catalog.GetString ("_Rehacer"), null, "gtk-redo");
		this.RedoAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Rehacer");
		w1.Add (this.RedoAction, "<Control>y");
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FiguresAction' action='FiguresAction'><menuitem name='AddEllipseAction' action='AddEllipseAction'/><menuitem name='AddRectangleAction' action='AddRectangleAction'/><menuitem name='AddPolyLineAction' action='AddPolyLineAction'/><menuitem name='AddSimpleTextAction' action='AddSimpleTextAction'/><menuitem name='AddMultiLineTextAction' action='AddMultiLineTextAction'/><menuitem name='AddLineConnectionAction' action='AddLineConnectionAction'/></menu><menu name='EditAction' action='EditAction'><menuitem name='UndoAction' action='UndoAction'/><menuitem name='RedoAction' action='RedoAction'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.mhdcanvas = new global::MonoHotDraw.SteticComponent ();
		this.mhdcanvas.Events = ((global::Gdk.EventMask)(256));
		this.mhdcanvas.Name = "mhdcanvas";
		this.vbox1.Add (this.mhdcanvas);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.mhdcanvas]));
		w3.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Zoom:");
		this.hbox2.Add (this.label2);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.label2]));
		w4.Position = 0;
		w4.Expand = false;
		w4.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.zoomscale = new global::Gtk.HScale (null);
		this.zoomscale.CanFocus = true;
		this.zoomscale.Name = "zoomscale";
		this.zoomscale.Adjustment.Lower = 10;
		this.zoomscale.Adjustment.Upper = 500;
		this.zoomscale.Adjustment.PageIncrement = 50;
		this.zoomscale.Adjustment.StepIncrement = 1;
		this.zoomscale.Adjustment.Value = 100;
		this.zoomscale.DrawValue = true;
		this.zoomscale.Digits = 0;
		this.zoomscale.ValuePos = ((global::Gtk.PositionType)(3));
		this.hbox2.Add (this.zoomscale);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.zoomscale]));
		w5.Position = 1;
		// Container child hbox2.Gtk.Box+BoxChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Visible Area:");
		this.hbox2.Add (this.label3);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.label3]));
		w6.Position = 2;
		w6.Expand = false;
		w6.Fill = false;
		this.hbox1.Add (this.hbox2);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.hbox2]));
		w7.Position = 0;
		// Container child hbox1.Gtk.Box+BoxChild
		this.visiblearea = new global::Gtk.Label ();
		this.visiblearea.Name = "visiblearea";
		this.visiblearea.LabelProp = global::Mono.Unix.Catalog.GetString ("~");
		this.hbox1.Add (this.visiblearea);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.visiblearea]));
		w8.Position = 1;
		w8.Expand = false;
		w8.Fill = false;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
		w9.Position = 2;
		w9.Expand = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 446;
		this.DefaultHeight = 300;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.AddEllipseAction.Activated += new global::System.EventHandler (this.OnAddEllipseActionActivated);
		this.AddRectangleAction.Activated += new global::System.EventHandler (this.OnAddRectangleActionActivated);
		this.AddPolyLineAction.Activated += new global::System.EventHandler (this.OnAddPolyLineActionActivated);
		this.AddSimpleTextAction.Activated += new global::System.EventHandler (this.OnAddSimpleTextActionActivated);
		this.AddMultiLineTextAction.Activated += new global::System.EventHandler (this.OnAddMultiLineTextActionActivated);
		this.AddLineConnectionAction.Activated += new global::System.EventHandler (this.OnAddLineConnectionActionActivated);
		this.UndoAction.Activated += new global::System.EventHandler (this.OnUndoActionActivated);
		this.RedoAction.Activated += new global::System.EventHandler (this.OnRedoActionActivated);
		this.zoomscale.ValueChanged += new global::System.EventHandler (this.OnHscale1ValueChanged);
	}
}
