// ===============================
// QUILL REGISTRY
// ===============================
window.quillEditors = window.quillEditors || {};

// ===============================
// INIT QUILL
// ===============================
window.initQuill = (editorId) => {
    const el = document.getElementById(editorId);
    if (!el) return;

    // Destroy previous instance safely
    if (window.quillEditors[editorId]) {
        delete window.quillEditors[editorId];
    }

    el.innerHTML = "";

    const quill = new Quill(el, {
        theme: "snow",
        placeholder: "Add a title…\n\nStart writing your entry here…",
        modules: {
            toolbar: [
                ["bold", "italic", "underline"],
                [{ header: [1, 2, false] }],
                [{ list: "ordered" }, { list: "bullet" }],
                ["clean"]
            ]
        }
    });

    window.quillEditors[editorId] = quill;
};

// ===============================
// GET CONTENT
// ===============================
window.getQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";
    return quill.root.innerHTML;
};

// ===============================
// SET CONTENT (for edit mode)
// ===============================
window.setQuillContent = (editorId, html) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    if (!html || html === "<p><br></p>") {
        quill.setText("");
    } else {
        quill.clipboard.dangerouslyPasteHTML(html);
    }
};

// ===============================
// CLEAR EDITOR
// ===============================
window.clearQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    quill.setText("");
    quill.setSelection(0);
};

// ===============================
// SUCCESS POPUP
// ===============================
window.showSaveSuccess = () => {
    alert("Journal entry saved successfully ✅");
};
// ===============================
// Quill Text
// ===============================
window.getQuillText = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";
    return quill.getText().trim();
};
// ===============================
// Get Title
// ===============================
window.getQuillTitle = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    const h1 = quill.root.querySelector("h1");
    if (h1 && h1.innerText.trim()) {
        return h1.innerText.trim();
    }

    const text = quill.getText().trim();
    return text ? text.split("\n")[0] : "";
};

