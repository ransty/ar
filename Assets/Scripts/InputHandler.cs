using UnityEngine;

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

    bool m_itemPicked = false;

    void Update()
    {
        CheckInput();
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
                obj.GetComponent<MeshRenderer>().material = gameObj.GetComponent<MeshRenderer>().material;
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
        if (m_itemPicked)
        {
            GameObject obj = m_itemHolder.GetChild(0).gameObject;
            if(obj)
                Destroy(obj);
            m_itemPicked = false;
        }
    }
}