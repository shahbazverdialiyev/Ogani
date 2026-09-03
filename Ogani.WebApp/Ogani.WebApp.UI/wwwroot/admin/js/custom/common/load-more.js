/**
 * Generic Load More function 
 * @param {HTMLElement} btnElement - The clicked "MORE" card element (this)
 * @param {number} chunkSize - Number of items to reveal per click
 * @param {string} itemLabel - Text label displayed in the remaining counter
 */
function loadMoreItems(btnElement, chunkSize = 18, itemLabel = 'products') {
    // Locate the parent container containing all item cards
    const container = btnElement.parentElement;
    if (!container) return;

    // Filter out hidden sibling elements that contain the 'd-none' class
    const hiddenItems = Array.from(container.children).filter(child =>
        child !== btnElement && child.classList.contains('d-none')
    );

    // Reveal the next chunk of hidden items
    const itemsToShow = hiddenItems.slice(0, chunkSize);
    itemsToShow.forEach(item => item.classList.remove('d-none'));

    // Calculate remaining hidden items
    const remainingCount = hiddenItems.length - itemsToShow.length;
    const remainingCounter = btnElement.querySelector('.remaining-count');

    if (remainingCount > 0) {
        if (remainingCounter) {
            remainingCounter.innerText = '+' + remainingCount + ' ' + itemLabel;
        }
    } else {
        // Hide the "MORE" button completely when no hidden items remain
        btnElement.classList.remove('d-flex');
        btnElement.classList.add('d-none');
        btnElement.setAttribute('style', 'display: none !important;');
    }
}