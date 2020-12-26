package crc64777a4b44392bce52;


public class ActcadastroColaborador
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("TESTEWL.Activitys.ActcadastroColaborador, TESTEWL", ActcadastroColaborador.class, __md_methods);
	}


	public ActcadastroColaborador ()
	{
		super ();
		if (getClass () == ActcadastroColaborador.class)
			mono.android.TypeManager.Activate ("TESTEWL.Activitys.ActcadastroColaborador, TESTEWL", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
