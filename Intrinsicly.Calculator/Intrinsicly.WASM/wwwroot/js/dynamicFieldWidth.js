window.setDynamicWidth = function (element) {
    if (element) {
        element.style.width = 'auto'; // Reset width to auto
        element.style.width = (element.scrollWidth + 2) + 'px'; // Set width to scroll width plus a small buffer
    }
}