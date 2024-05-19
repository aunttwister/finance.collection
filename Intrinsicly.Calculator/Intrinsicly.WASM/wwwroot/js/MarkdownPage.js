// MarkdownPage.js - JavaScript for handling interactive markdown features

document.addEventListener("DOMContentLoaded", function () {
    initializeExpandableList();
});

function initializeExpandableList() {
    document.querySelectorAll(".markdown-page ul li").forEach(function (li) {
        if (li.querySelector("ul")) {
            li.classList.add("collapsible");
            li.classList.add("collapsed");
            li.addEventListener("click", function (e) {
                li.classList.toggle("collapsed");
                e.stopPropagation();
            });
        }
    });
}

window.initializeMarkdownPage = function () {
    initializeExpandableList();
};
