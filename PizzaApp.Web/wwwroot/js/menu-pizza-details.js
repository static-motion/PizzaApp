document.addEventListener('DOMContentLoaded', () => {

    const pizzaForm = document.forms['pizza-form'];
    const minusBtn = document.querySelector('#minus-btn');
    const plusBtn = document.querySelector('#plus-btn');
    const quantityInput = document.querySelector('#quantity-input');
    const totalPriceOutput = document.querySelector('#total-price');
    const basePrice = parseFloat(document.querySelector('#base-price').value);

    let sum = basePrice;

    function updateTotalPrice() {
        const quantity = parseInt(quantityInput.value, 10);
        if (isNaN(quantity) || quantity < 1) {
            return; // Don't update if quantity is invalid
        }
        const total = sum * quantity;
        totalPriceOutput.textContent = '$' + total.toFixed(2);
    }

    minusBtn.addEventListener('click', function () {
        let currentValue = parseInt(quantityInput.value, 10);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
            updateTotalPrice();
        }
    });


    plusBtn.addEventListener('click', function () {
        let currentValue = parseInt(quantityInput.value, 10);
        quantityInput.value = currentValue + 1;
        updateTotalPrice();
    });


    quantityInput.addEventListener('input', function () {
        // Ensure the value is not empty or less than 1
        if (quantityInput.value === '' || parseInt(quantityInput.value, 10) < 1) {
            quantityInput.value = 1;
        }
        updateTotalPrice();
    });


    quantityInput.addEventListener('change', () => {
        updateTotalPrice();
    })

    // Ensure the initial total is correct (it's set by the server, but good practice)
    updateTotalPrice();

    pizzaForm.reset();


    pizzaForm.addEventListener('input', () => {
        sum = 0;
        pizzaForm.querySelectorAll('input.form-check-input').forEach(box => {
            if (box.checked) sum += +box.getAttribute('data-price');
        })

        pizzaForm.querySelectorAll('.form-select').forEach(select => {
            const option = select.options[select.selectedIndex]
            if (option) {
                console.log(option.getAttribute('data-price'));
                sum += +option.getAttribute('data-price');
            }
        })
        updateTotalPrice();
    });
    pizzaForm.onsubmit = e => {
        //e.preventDefault()  // disable submit
    }
});