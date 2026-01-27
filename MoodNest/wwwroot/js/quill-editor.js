// =====================================================
// QUILL EDITOR REGISTRY
// -----------------------------------------------------
// Acts as a global, safe store for active Quill instances
// keyed by editor DOM element ID.
// Prevents duplicate editors and enables reuse.
// =====================================================
window.quillEditors = window.quillEditors || {};


// =====================================================
// INITIALIZE QUILL EDITOR
// -----------------------------------------------------
// Creates a new Quill editor instance for the given
// element ID. If an editor already exists for this ID,
// it is safely replaced.
// =====================================================
window.initQuill = (editorId) => {
    const el = document.getElementById(editorId);
    if (!el) return;

    // Remove any existing editor instance for this element
    if (window.quillEditors[editorId]) {
        delete window.quillEditors[editorId];
    }

    // Clear existing DOM content before initializing
    el.innerHTML = "";

    // Create Quill editor with constrained toolbar
    const quill = new Quill(el, {
        theme: "snow",
        placeholder: "Use Heading 1 for title...\nStart writing your entry...",
        modules: {
            toolbar: [
                [{ header: [1, 2, false] }],   // H1 = Title, H2 = Section
                ["bold", "italic", "underline"],
                [{ list: "ordered" }, { list: "bullet" }],
                ["clean"]
            ]
        }
    });

    // Store instance in global registry
    window.quillEditors[editorId] = quill;
};


// =====================================================
// EXTRACT TITLE (FIRST H1 ONLY)
// -----------------------------------------------------
// Retrieves the first <h1> element from the editor,
// which is treated as the journal title.
// =====================================================
window.getQuillTitle = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    const h1 = quill.root.querySelector("h1");
    return h1 ? h1.innerText.trim() : "";
};


// =====================================================
// EXTRACT BODY CONTENT (WITHOUT TITLE)
// -----------------------------------------------------
// Returns editor HTML content excluding the first <h1>.
// This ensures the title and body are stored separately.
// =====================================================
window.getQuillBody = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    // Clone editor DOM to avoid mutating live content
    const clone = quill.root.cloneNode(true);

    // Remove only the first H1 (title)
    const h1 = clone.querySelector("h1");
    if (h1) h1.remove();

    return clone.innerHTML.trim();
};


// =====================================================
// SET EDITOR CONTENT (EDIT MODE SAFE)
// -----------------------------------------------------
// Reconstructs editor content by combining title and
// body HTML. Used when loading an existing journal entry.
// =====================================================
window.setQuillContent = (editorId, title = "", bodyHtml = "") => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    let html = "";

    // Insert title as H1 if present
    if (title && title.trim() !== "") {
        html += `<h1>${title}</h1>`;
    }

    // Append body HTML if present
    if (bodyHtml && bodyHtml.trim() !== "") {
        html += bodyHtml;
    }

    // Safely inject reconstructed HTML into editor
    quill.clipboard.dangerouslyPasteHTML(html);
};


// =====================================================
// CLEAR EDITOR CONTENT
// -----------------------------------------------------
// Resets the editor to an empty state and places the
// cursor at the beginning.
// =====================================================
window.clearQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    quill.setText("");
    quill.setSelection(0);
};


// =====================================================
// SAVE SUCCESS FEEDBACK
// -----------------------------------------------------
// Provides simple user feedback after successful save.
// =====================================================
window.showSaveSuccess = () => {
    alert("Journal entry saved successfully ✅");
};


// =====================================================
// WORD COUNT (ON DEMAND)
// -----------------------------------------------------
// Calculates word count by extracting plain text from
// the editor and splitting on whitespace.
// =====================================================
window.getQuillWordCount = function (editorId) {
    const quill = Quill.find(document.getElementById(editorId));
    if (!quill) return 0;

    const text = quill.getText().trim();
    if (!text) return 0;

    return text.split(/\s+/).length;
};


// =====================================================
// LIVE WORD COUNT REGISTRATION
// -----------------------------------------------------
// Registers a listener for text changes and sends
// updated word count to Blazor via JS interop.
// =====================================================
window.registerQuillWordCounter = function (editorId, dotnetRef) {
    const quill = Quill.find(document.getElementById(editorId));
    if (!quill) return;

    quill.on("text-change", function () {
        const text = quill.getText().trim();
        const count = text ? text.split(/\s+/).length : 0;

        // Invoke Blazor method asynchronously
        dotnetRef.invokeMethodAsync("UpdateWordCount", count);
    });
};
