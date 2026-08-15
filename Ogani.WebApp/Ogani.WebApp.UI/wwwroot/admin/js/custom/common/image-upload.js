/**
 * Dynamic & Reusable Image Upload Preview & Remove Utility
 */

function previewImage(input) {
    if (input.files && input.files[0]) {
        // İnpudun yerləşdiyi ən yaxın form-group və ya container-i tapırıq
        const container = input.closest('.form-group') || input.closest('.image-upload-wrapper');
        if (!container) return;

        const preview = container.querySelector('.image-preview-img');
        const placeholder = container.querySelector('.upload-placeholder');
        const removeBtn = container.querySelector('.remove-img-btn');
        const removeFlag = container.querySelector('.remove-existing-image-flag');

        const reader = new FileReader();

        reader.onload = function (e) {
            if (preview) {
                preview.src = e.target.result;
                preview.classList.remove('d-none');
            }
            if (placeholder) {
                placeholder.classList.add('d-none');
            }
            if (removeBtn) {
                removeBtn.classList.remove('d-none');
            }
            if (removeFlag) {
                removeFlag.value = 'false';
            }
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function removeSelectedImage(button, event) {
    if (event) event.stopPropagation();

    const container = button.closest('.form-group') || button.closest('.image-upload-wrapper');
    if (!container) return;

    const preview = container.querySelector('.image-preview-img');
    const placeholder = container.querySelector('.upload-placeholder');
    const removeBtn = container.querySelector('.remove-img-btn');
    const input = container.querySelector('input[type="file"]');
    const removeFlag = container.querySelector('.remove-existing-image-flag');

    if (preview) {
        preview.src = '#';
        preview.classList.add('d-none');
    }
    if (placeholder) {
        placeholder.classList.remove('d-none');
    }
    if (removeBtn) {
        removeBtn.classList.add('d-none');
    }
    if (input) {
        input.value = '';
    }
    if (removeFlag) {
        removeFlag.value = 'true';
    }
}

function triggerFileInput(container) {
    const fileInput = container.querySelector('input[type="file"]');
    if (fileInput) {
        fileInput.click();
    }
}