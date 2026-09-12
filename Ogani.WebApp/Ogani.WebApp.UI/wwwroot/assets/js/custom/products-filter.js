// 1. Qlobal Filter State Obyekti (URL-dən ilkin dəyərləri oxuyuruq)
const urlParams = new URLSearchParams(window.location.search);

const filterState = {
    Search: urlParams.get("Search") || "",
    CategoryId: urlParams.get("CategoryId") || "",
    MinPrice: urlParams.get("MinPrice") || "",
    MaxPrice: urlParams.get("MaxPrice") || "",
    SortBy: urlParams.get("SortBy") || "default",
    Page: parseInt(urlParams.get("Page")) || 1
};

const defaultValues = [null, "", undefined, "default"]

$(document).ready(function () {

    history.replacestate({ ...filterstate }, "", window.location.href);

    loadProducts();

});

function loadProducts(actionName = "Products", destination = "#product-list", pushHistory = true) {
    let cleanParams = {};

    for (let key in filterState) {
        if (!defaultValues.includes(filterState[key])) {
            cleanParams[key] = filterState[key];
        }
    }

    let queryString = $.param(cleanParams);
    let newUrl = window.location.pathname + (queryString ? "?" + queryString : "");

    if (pushHistory && window.location.search !== (queryString ? "?" + queryString : "")) {
        window.history.pushState(filterState, "", newUrl);
    }

    // Action adı parametr olaraq URL-ə yazılır
    $.ajax({
        url: `/Shop/${actionName}?` + queryString,
        type: "GET",
        success: function (result) {
            $(destination).html(result);

            // UI bərpaları
            initProducts(destination);

            // Əgər bütün row yüklənibsə, slider-i da təzələyirik

        },
        error: function (xhr) {
            console.error("AJAX Error:", xhr);
        }
    });
}

// Helper: NiceSelect Bərpası VƏ Xəta Yoxlaması (Safely Check)
function initProducts(container) {
    if ($.fn.niceSelect) {
        $(container).find("select").niceSelect();
    }
    if ($(container).find(".price-range")) {
        initPriceRange();
    }
}

// Helper: Price Range Slider Bərpası
function initPriceRange() {
    let $slider = $(".price-range");
    if ($slider.length === 0 || !$.fn.slider) return;

    let minPrice = $slider.data("min") || 0;
    let maxPrice = $slider.data("max") || 1000;

    $slider.slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [
            filterState.MinPrice ? filterState.MinPrice : minPrice,
            filterState.MaxPrice ? filterState.MaxPrice : maxPrice
        ],
        slide: function (event, ui) {
            $("#minamount").val("$" + ui.values[0]);
            $("#maxamount").val("$" + ui.values[1]);
        },
        stop: function (event, ui) {
            filterState.MinPrice = ui.values[0];
            filterState.MaxPrice = ui.values[1];
            filterState.Page = 1;
            loadProducts();
        }
    });
    $("#minamount").val('$' + $slider.slider("values", 0));
    $("#maxamount").val('$' + $slider.slider("values", 1));
}

// ================= EVENT LISTENER-LƏR =================

// A) SEARCH FORM SUBMIT (Home səhifəsində normal redirect, Shop-da AJAX çalışacaq)
$(document).on("submit", "#shop-search-form", function (e) {
    if (window.location.pathname.toLowerCase().includes("/shop")) {
        e.preventDefault();

        filterState.Search = $(this).find("input[name='Search']").val();

        filterState.CategoryId = "";
        filterState.SortBy = "";
        filterState.MinPrice = "";
        filterState.MaxPrice = "";
        filterState.Page = "";

        loadProducts("ProductsRow", "#products-row");
    }
});

// B) KATEQORİYA SEÇİMİ
$(document).on("click", ".category-filter", function (e) {
    e.preventDefault();

    let categoryId = $(this).data("category-id");

    $(".category-filter").parent().removeClass("category-selected").addClass("category-list-item");

    if (filterState.CategoryId != categoryId) {

        $(this).parent().addClass("category-selected").removeClass("category-list-item");

        filterState.CategoryId = categoryId;
    }
    else {
        filterState.CategoryId = "";
    }

    filterState.SortBy = "";
    filterState.MinPrice = "";
    filterState.MaxPrice = "";
    filterState.Page = "";

    loadProducts("ProductsRow", "#products-row");
});

// C) ÇEŞİDLƏMƏ (SortBy - NiceSelect Dropdown)
$(document).on("change", ".filter__sort select", function () {
    filterState.SortBy = $(this).val();
    filterState.Page = 1;

    loadProducts();
});

// D) SƏHİFƏLƏMƏ (Pagination Links)
$(document).on("click", ".product__pagination a.page_link", function (e) {
    e.preventDefault();

    filterState.Page = $(this).data("page");
    loadProducts();
});

// E) BRAUZERİN BACK / FORWARD DÜYMƏLƏRİ İLƏ İŞLƏMƏK (Popstate)
window.onpopstate = function (event) {
    if (event.state) {
        Object.assign(filterState, event.state);
    } else {
        // İllin vəziyyətə qayıdırıq
        const currentParams = new URLSearchParams(window.location.search);
        filterState.Search = currentParams.get("Search") || "";
        filterState.CategoryId = currentParams.get("CategoryId") || "";
        filterState.MinPrice = currentParams.get("MinPrice") || "";
        filterState.MaxPrice = currentParams.get("MaxPrice") || "";
        filterState.SortBy = currentParams.get("SortBy") || "default";
        filterState.Page = parseInt(currentParams.get("Page")) || 1;
    }
    loadProducts("Products", "#product-list", false);
};