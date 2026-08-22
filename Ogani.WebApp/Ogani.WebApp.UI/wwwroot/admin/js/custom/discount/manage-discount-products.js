// Manage Discount Products Interactivity & Real-time Counter

document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".product-card");
    const countBadge = document.getElementById("selected-count-badge");
    const detailLinks = document.querySelectorAll(".product-detail-link");

    function updateSelectedCount() {
        let count = 0;
        cards.forEach(card => {
            const cb = card.querySelector(".product-checkbox");
            if (cb.checked) {
                count++;
                card.classList.add("selected-card");
            } else {
                card.classList.remove("selected-card");
            }
        });

        if (countBadge) {
            countBadge.innerText = count + (count === 1 ? " Product" : " Products");
        }
    }

    // Məhsul adının (linkin) üzərinə klikləyəndə kartın toggle olunmasının qarşısını alırıq
    detailLinks.forEach(link => {
        link.addEventListener("click", function (e) {
            e.stopPropagation();
        });
    });

    // Kartın istənilən yerinə kliklədikdə checkbox toggle olsun
    cards.forEach(card => {
        card.addEventListener("click", function () {
            const cb = this.querySelector(".product-checkbox");
            if (cb) {
                cb.checked = !cb.checked;
                updateSelectedCount();
            }
        });
    });

    // Səhifə yüklənəndə ilkin say hesablansın
    updateSelectedCount();
});