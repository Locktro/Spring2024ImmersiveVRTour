using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    [SerializeField] private GameObject checklistCanvas;        // The Canvas holding the checklist
    [SerializeField] private Transform checklistContainer;      // ScrollView Content to hold checklist items
    [SerializeField] private TextMeshProUGUI scoreText;         // TextMeshPro for the total score
    [SerializeField] private ScrollRect scrollView;             // Reference to the ScrollView component
    [SerializeField] private GameObject checklistItemPrefab;    // Prefab for checklist items

    private List<string> checklistItems = new List<string>      // Static list of checklist items
    {
        "Football",
        "Buzz Prison",
        "Ramen Bowl",
        "Band Hat",
    };

    private Dictionary<string, Toggle> checklistToggles = new Dictionary<string, Toggle>();  // Dictionary to map item names to their Toggles
    private int totalScore = 0;                                 // Total score or items found count

    private void Start()
    {
        PopulateChecklist();   // Populate using static checklistItems
        UpdateScoreDisplay();  // Initialize score display
        InitializeScrollView(); // Ensure the ScrollView starts at the top
    }

    // Populates the checklist from the static checklistItems list
    private void PopulateChecklist()
    {
        // Clear existing checklist items
        foreach (Transform child in checklistContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate checklist items dynamically
        foreach (string itemName in checklistItems)
        {
            // Instantiate the prefab for each checklist item
            GameObject newItem = Instantiate(checklistItemPrefab, checklistContainer);

            // Set the label text to the item name
            TextMeshProUGUI label = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
            {
                label.text = itemName;
            }

            // Get the Toggle component and store it in the dictionary for later reference
            Toggle toggle = newItem.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.isOn = false; // Ensure all toggles start unchecked
                checklistToggles[itemName] = toggle;

                // Optional: Add a listener to handle toggle changes (if needed later)
                toggle.onValueChanged.AddListener((isChecked) => OnToggleValueChanged(itemName, isChecked));
            }
        }

        UpdateContentSize();
    }

    // Updates the content size of the ScrollView dynamically
    private void UpdateContentSize()
    {
        if (checklistContainer.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            float totalHeight = 0f;

            // Calculate total height based on the children in the Vertical Layout Group
            foreach (Transform child in checklistContainer)
            {
                RectTransform childRect = child.GetComponent<RectTransform>();
                if (childRect != null)
                {
                    totalHeight += childRect.rect.height;
                }
            }

            // Add spacing from the Vertical Layout Group
            VerticalLayoutGroup layoutGroup = checklistContainer.GetComponent<VerticalLayoutGroup>();
            if (layoutGroup != null)
            {
                totalHeight += layoutGroup.spacing * (checklistItems.Count - 1);
                totalHeight += layoutGroup.padding.top + layoutGroup.padding.bottom;
            }

            // Set the container height
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
        }
    }

    // Ensure the ScrollView starts at the top
    private void InitializeScrollView()
    {
        if (scrollView != null)
        {
            scrollView.verticalNormalizedPosition = 1f; // Position ScrollView to top
        }
    }

    // Updates the score text display
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Items Found: " + totalScore;
        }
    }

    // Handles toggle value changes (optional functionality for future use)
    private void OnToggleValueChanged(string itemName, bool isChecked)
    {
        if (isChecked)
        {
            totalScore++;
        }
        else
        {
            totalScore--;
        }
        UpdateScoreDisplay();
    }

    // Mark an item as found when selected
    public void MarkItemOnSelect(GameObject selectedObject)
    {
        string itemName = selectedObject.name; // Use the object's name to find the checklist item
        
        if (checklistToggles.TryGetValue(itemName, out Toggle toggle))
        {
            if (!toggle.isOn) // Only mark if it's not already checked
            {
                toggle.isOn = true;  // Mark the toggle as checked
                totalScore++;       // Increment the score
                UpdateScoreDisplay(); // Update the UI score
                Debug.Log($"Item '{itemName}' has been marked as found.");
            }
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in the checklist.");
        }
    }
}