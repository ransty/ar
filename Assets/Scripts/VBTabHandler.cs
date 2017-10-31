using UnityEngine;
using Vuforia;

public class VBTabHandler : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject m_shapes;
    private GameObject m_textures;

    void Start()
    {
        // Register with the virtual buttons TrackableBehaviour
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
        {
            vbs[i].RegisterEventHandler(this);
        }

        m_shapes = transform.Find("Shapes").gameObject;
        m_textures = transform.Find("Textures").gameObject;
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

        switch (vb.VirtualButtonName)
        {
            case "Shape":
                m_shapes.SetActive(true);
                m_textures.SetActive(false);
                break;

            case "Texture":
                m_shapes.SetActive(false);
                m_textures.SetActive(true);
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("OnButtonReleased");
    }

}
