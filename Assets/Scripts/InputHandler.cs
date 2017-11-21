using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public Transform m_itemHolder;
    public Transform m_sceneSpace;
    public float m_DoubleClickTime = 0.3f;
    private int m_NumClick = 0;
    private float m_TimePassed = 0f;
    private Vector2 m_MouseDownPosition;
    private Vector2 m_MouseUpPosition;
    private float m_LastMouseUpTime;
    public bool group = false;
    bool m_itemPicked = false;

    public GameObject slider;
    public GameObject rotate;
    public Color color;
    public GameObject rslider;
    public GameObject gslider;
    public GameObject bslider;

    private bool xAxis = true;
    private bool yAxis = false;
    private bool zAxis = false;

    private float objScale = 0;

    GameObject Shapes;
    GameObject Textures;
    GameObject[] shapeList;

    private GUIStyle style;

    private void Awake()
    {
        Shapes = GameObject.Find("Shapes");
        shapeList = GameObject.FindGameObjectsWithTag("ShapeTemplate");
        Textures = GameObject.Find("Textures");
        Textures.SetActive(false);
        style = new GUIStyle();
    }

    void OnGUI() // These functions don't really work well on phone screens
    {
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Group = " + group + "\nChild Count: " + m_itemHolder.childCount, style);

    }

    void Update()
    {
        CheckInput();

        // Scale
        if (m_itemPicked)
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            obj.transform.localScale = new Vector3(slider.GetComponent<Slider>().value, slider.GetComponent<Slider>().value, slider.GetComponent<Slider>().value);
        }

        // Rotate
        if (m_itemPicked)
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            if (xAxis)
            {
                obj.transform.eulerAngles = new Vector3(rotate.GetComponent<Slider>().value, 0, 0);
            } else if (yAxis)
            {
                obj.transform.eulerAngles = new Vector3(0, rotate.GetComponent<Slider>().value, 0);
            } else if (zAxis)
            {
                obj.transform.eulerAngles = new Vector3(0, 0, rotate.GetComponent<Slider>().value);
            }
        }

        // Single Colour Picker
        if (!group)
        {
            color = new Color(rslider.GetComponent<Slider>().value, gslider.GetComponent<Slider>().value, bslider.GetComponent<Slider>().value);
            if (m_itemPicked) //destroy object in hand first
            {
                m_itemHolder.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }

    /*
     * 
     * Change to x axis for rotation
     */
    public void ChangeToXAxis()
    {
        xAxis = true;
        yAxis = false;
        zAxis = false;
        // Set rotate text to (X)
        GameObject text = GameObject.FindGameObjectWithTag("RotateText");
        text.GetComponent<Text>().text = "(X) Rotate";
    }

    /*
    * 
    * Change to y axis for rotation
    */
    public void ChangeToYAxis()
    {
        xAxis = false;
        yAxis = true;
        zAxis = false;
        // Set rotate text to (Y)
        GameObject text = GameObject.FindGameObjectWithTag("RotateText");
        text.GetComponent<Text>().text = "(Y) Rotate";
    }

    /*
    * 
    * Change to z axis for rotation
    */
    public void ChangeToZAxis()
    {
        xAxis = false;
        yAxis = false;
        zAxis = true;
        // Set rotate text to (Z)
        GameObject text = GameObject.FindGameObjectWithTag("RotateText");
        text.GetComponent<Text>().text = "(Z) Rotate";
    }


    private void CheckInput()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            m_MouseDownPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            m_NumClick++;
            m_LastMouseUpTime = Time.time;
        }

        if (m_NumClick > 0)
        {
            if (m_TimePassed > m_DoubleClickTime) //If the max duration for a double click has passed then it's a single click
            {
                HandleClick();
                m_TimePassed = 0;
                m_NumClick = 0;
            }
            else
            {
                if (m_NumClick > 1) //Multiple click can be handled here but we just classify all of them as double click
                {
                    //Handle 2 or more clicks
                    m_TimePassed = 0;
                    m_NumClick = 0;
                }
            }
            m_TimePassed += Time.deltaTime; //Keep track of time passed after a click
                                            //Debug.Log(m_TimePassed);
        }
    }

    void HandleClick()
    {
        Vector3 position = Input.mousePosition;
        Ray inputRay = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            GameObject gameObj = hit.transform.gameObject;
            if (gameObj.tag == "ShapeTemplate")
                HandleShapeTemplate(gameObj);
            else if (gameObj.tag == "TextureTemplate")
                HandleTextureTemplate(gameObj);
            else if (gameObj.tag == "Interactive")
                HandleInteractive(gameObj);
        }
    }

    void HandleShapeTemplate(GameObject gameObj)
    {
        if (m_itemPicked) //destroy object in hand first
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            if (obj)
                Destroy(obj);
        }
        GameObject newObj = (GameObject)Instantiate(gameObj);
        newObj.transform.SetParent(m_itemHolder, false);
        newObj.transform.rotation = gameObj.transform.rotation;
        newObj.tag = "Interactive";
        m_itemPicked = true;
    }

    void HandleTextureTemplate(GameObject gameObj)
    {
        if (m_itemPicked)
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            if (obj)
            {
                obj.GetComponent<MeshRenderer>().material.mainTexture = gameObj.GetComponent<MeshRenderer>().material.mainTexture;
            }
        }

        if (group)
        {
            foreach (GameObject shape in shapeList)
            {
                shape.GetComponent<MeshRenderer>().material.mainTexture = gameObj.GetComponent<MeshRenderer>().material.mainTexture;
            }
        }

    }

    void HandleInteractive(GameObject gameObj)
    {
        if (m_itemPicked) //picked up
        {
            gameObj.transform.SetParent(m_sceneSpace);
            m_itemPicked = false;
        }
        else
        {
            gameObj.transform.SetParent(m_itemHolder);
            m_itemPicked = true;
        }
    }

    public void DeleteObject()
    {
        if (m_itemPicked && !group)
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            if (obj)
                Destroy(obj);
            m_itemPicked = false;
        }

        if (m_itemPicked && group) // group copy
        {
            for (int i = 0; i < m_itemHolder.childCount; i++)
            {
                GameObject go = m_itemHolder.GetChild(i).gameObject;
                if (go)
                    Destroy(go);
            }
        }
    }

    public void CopyObject()
    {
        if (m_itemPicked && !group)
        {
            GameObject copy = m_itemHolder.GetChild(0).gameObject;
            if (copy)
                Instantiate(copy, copy.transform.position, copy.transform.rotation);
            m_itemPicked = false;
        }

        if (m_itemPicked && group) // group copy
        {
            for (int i = 0; i < m_itemHolder.childCount; i++)
            {
                GameObject go = m_itemHolder.GetChild(i).gameObject;
                if (go)
                    Instantiate(go, go.transform.position, go.transform.rotation);
            }
        }
    }

    public void DropAllObject()
    {
        if (group)
        {
            for (int i = m_itemHolder.childCount - 1; i >= 0; --i)
            {
                Transform child = m_itemHolder.GetChild(i);
                child.SetParent(m_sceneSpace);
            }
        }
        else
        {
            m_itemHolder.GetChild(0).SetParent(m_sceneSpace);
        }
    }

    public void ScaleObject()
    {
        for (int i = m_itemHolder.childCount - 1; i >= 0; i--)
        {
            Transform child = m_itemHolder.GetChild(i);
            child.localScale += new Vector3(0, 1, 0);
        }
    }

    public void SetGroup()
    {
        group = !group;
        Image gButton = GameObject.FindGameObjectWithTag("GroupButton").GetComponent<Image>();
        if (group)
        {
            gButton.color = new Color32(255, 255, 225, 100);
        }
        else
        {
            gButton.color = new Color32(26, 179, 255, 203);
        }
    }
}