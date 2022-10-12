using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class RayCharacter : MonoBehaviour
{
    [Header("��������� ��������������")]
    [SerializeField] private float maxDistance;
    public Image imageE;
    public Image imageT;
    public Button buttonE;
    public Button buttonT;
    private Ray ray;
    private RaycastHit hit;

    [Header("������ ��������������")]
    [SerializeField] private float radius;
    [SerializeField] private bool isGizmos = true;

    [Header("���������")]
    public Transform inventoryPanel;
    public List<Slot> slots = new List<Slot>();
    public CinemachineFreeLook freeLook;
    [SerializeField] public bool isOpenPanel;

    private CharacterMove characterMove;

    private void Start()
    {
        freeLook.m_XAxis.Value = 0.5f;
        freeLook.m_YAxis.Value = 0.5f;

        // ��������� �������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterMove = GetComponent<CharacterMove>();

        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<Slot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<Slot>());
            }
        }
    }

    private void Ray()
    {
        ray = new Ray(transform.position + new Vector3(0, 1f, 0), transform.forward);
    }

    private void DrawRay()
    {
        if(Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue);
        }
        
        if(hit.transform == null)
        {
            if (characterMove.move == CharacterMove.Move.PC)
            {
                imageE.enabled = false;
                imageT.enabled = false;

            }
            else if (characterMove.move == CharacterMove.Move.Android)
{
                buttonE.image.enabled = false;
                buttonT.image.enabled = false;
            }
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }
    }

    private void Interact() // ����� �������������� � ���������� 
    {
        if (hit.transform != null && hit.transform.GetComponent<Interactions>())
        {
            if (characterMove.move == CharacterMove.Move.PC)
            {
                imageE.enabled = true;
                imageT.enabled = true;

            }
            else if (characterMove.move == CharacterMove.Move.Android)
            {
                buttonE.image.enabled = true;
                buttonT.image.enabled = true;
            }
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }
    }

    private void Radius() // ����� �������������� � ��������� ��� ������������
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for(int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if(rigidbody)
            {
                if (characterMove.move == CharacterMove.Move.PC)
                {
                    imageE.enabled = true;
                    imageT.enabled = true;

                }
                else if (characterMove.move == CharacterMove.Move.Android)
                {
                    buttonE.image.enabled = true;
                    buttonT.image.enabled = true;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    rigidbody.GetComponent<Interactions>().PickUp();
                }
                if(Input.GetKeyDown(KeyCode.F))
                {
                    if(rigidbody.gameObject.GetComponent<ItemPrefab>().food != null) { AddItemFood(rigidbody.gameObject.GetComponent<ItemPrefab>().food, rigidbody.gameObject.GetComponent<ItemPrefab>().amount); }
                    else { AddItem(rigidbody.gameObject.GetComponent<ItemPrefab>().item, rigidbody.gameObject.GetComponent<ItemPrefab>().amount); }
                    Destroy(rigidbody.gameObject);
                }
            }
            
        }
    }

    private void AddItem(Item _item, int _amount)
    {
        foreach (Slot slot in slots)
        {
            if(slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maxAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmount.text = slot.amount.ToString();
                    return;
                }
                break;
            }
        }
        foreach (Slot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmount.text = _amount.ToString();
                break;
            }
        }
    }

    private void AddItemFood(FoodItem _item, int _amount)
    {
        foreach (Slot slot in slots)
        {
            if (slot.foodItem == _item)
            {
                if (slot.amount + _amount <= _item.maxAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmount.text = slot.amount.ToString();
                    return;
                }
                break;
            }
        }
        foreach (Slot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.foodItem = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmount.text = _amount.ToString();
                break;
            }
        }
    }

    private void ScrollCamera()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1)
        {
            if(freeLook.m_Lens.FieldOfView >= 25) { freeLook.m_Lens.FieldOfView -= 5; }
        }
        else if (mw < -0.1)
        {
            if (freeLook.m_Lens.FieldOfView <= 45) { freeLook.m_Lens.FieldOfView += 5; }
        }

    }

    public void ButtonE()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.GetComponent<Interactions>().PickUp();
            }

        }
    }

    public void ButtonT()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.GetComponent<Interactions>().Release();
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (isGizmos == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(0, 1f, 0), radius);
        }
    }

    private void Update()
    {
        Ray();
        DrawRay();
        Interact();
        Radius();

        ScrollCamera();

        if (characterMove.move == CharacterMove.Move.PC)
        {
            buttonE.gameObject.SetActive(false);
            buttonT.gameObject.SetActive(false);
            imageE.gameObject.SetActive(true);
            imageT.gameObject.SetActive(true);
            
        }
        else if (characterMove.move == CharacterMove.Move.Android)
        {
            imageE.gameObject.SetActive(false);
            imageT.gameObject.SetActive(false);
            buttonE.gameObject.SetActive(true);
            buttonT.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpenPanel == true)
            {
                inventoryPanel.gameObject.SetActive(false);
                isOpenPanel = false;
                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
                characterMove.charMenegment = true;
                // ��������� �������
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (isOpenPanel == false)
            {
                inventoryPanel.gameObject.SetActive(true);
                isOpenPanel = true;
                freeLook.m_XAxis.m_InputAxisName = "";
                freeLook.m_XAxis.m_InputAxisValue = 0;
                freeLook.m_YAxis.m_InputAxisName = "";
                freeLook.m_YAxis.m_InputAxisValue = 0;
                characterMove.charMenegment = false;
                // ��������� �������
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
