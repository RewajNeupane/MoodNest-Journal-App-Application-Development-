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

    if (window.quillEditors[editorId]) {
        delete window.quillEditors[editorId];
    }

    el.innerHTML = "";

    const quill = new Quill(el, {
        theme: "snow",
        placeholder: "Use Heading 1 for title...\nStart writing your entry...",
        modules: {
            toolbar: [
                [{ header: [1, 2, false] }], // ✅ H1 = Title, H2 = Section
                ["bold", "italic", "underline"],
                [{ list: "ordered" }, { list: "bullet" }],
                ["clean"]
            ]
        }
    });

    window.quillEditors[editorId] = quill;
};

// ===============================
// GET FULL HTML
// ===============================
window.getQuillContent = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";
    return quill.root.innerHTML.trim();
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
// GET BODY (REMOVE ONLY H1, KEEP H2)
// ===============================
window.getQuillBody = (editorId) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return "";

    const clone = quill.root.cloneNode(true);

    // 🔴 REMOVE ONLY FIRST H1
    const h1 = clone.querySelector("h1");
    if (h1) h1.remove();

    return clone.innerHTML.trim();
};

// ===============================
// SET CONTENT (EDIT MODE)
// ===============================
window.setQuillContent = (editorId, title, bodyHtml) => {
    const quill = window.quillEditors[editorId];
    if (!quill) return;

    let html = "";

    if (title) {
        html += `<h1>${title}</h1>`;
    }

    if (bodyHtml) {
        html += bodyHtml;
    }

    quill.clipboard.dangerouslyPasteHTML(html);
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
