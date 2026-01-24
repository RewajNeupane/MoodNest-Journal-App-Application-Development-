// ===============================
// QUILL REGISTRY (GLOBAL SAFE STORE)
// ===============================
window.quillEditors = window.quillEditors || {};

// ===============================
// INIT QUILL EDITOR
// ===============================
window.initQuill = (editorId) => {
    const el = document.getElementById(editorId);
    if (!el) return;

    // Destroy old instance if exists
    if (window.quillEditors[editorId]) {
        delete window.quillEditors[editorId];
    }

    el.innerHTML = "";

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

    window.quillEditors[editorId] = quill;
};

// ===============================
// GET TITLE (FIRST H1 ONLY)
// ===============================
window.getQuillTitle = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    const h1 = quill.root.querySelector("h1");
    return h1 ? h1.innerText.trim() : "";
};

// ===============================
// GET BODY HTML (REMOVE ONLY FIRST H1)
// ===============================
window.getQuillBody = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    const clone = quill.root.cloneNode(true);

    // Remove ONLY the first H1 (title)
    const h1 = clone.querySelector("h1");
    if (h1) h1.remove();

    return clone.innerHTML.trim();
};

// ===============================
// SET CONTENT (EDIT MODE SAFE)
// ===============================
window.setQuillContent = (editorId, title = "", bodyHtml = "") => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    let html = "";

    if (title && title.trim() !== "") {
        html += `<h1>${title}</h1>`;
    }

    if (bodyHtml && bodyHtml.trim() !== "") {
        html += bodyHtml;
    }

    quill.clipboard.dangerouslyPasteHTML(html);
};

// ===============================
// CLEAR EDITOR CONTENT
// ===============================
window.clearQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    quill.setText("");
    quill.setSelection(0);
};

// ===============================
// SUCCESS FEEDBACK
// ===============================
window.showSaveSuccess = () => {
    alert("Journal entry saved successfully ✅");
};

window.getQuillWordCount = function (editorId) {
    const quill = Quill.find(document.getElementById(editorId));
    if (!quill) return 0;

    const text = quill.getText().trim();
    if (!text) return 0;

    return text.split(/\s+/).length;
};

window.registerQuillWordCounter = function (editorId, dotnetRef) {
    const quill = Quill.find(document.getElementById(editorId));
    if (!quill) return;

    quill.on('text-change', function () {
        const text = quill.getText().trim();
        const count = text ? text.split(/\s+/).length : 0;
        dotnetRef.invokeMethodAsync("UpdateWordCount", count);
    });
};

