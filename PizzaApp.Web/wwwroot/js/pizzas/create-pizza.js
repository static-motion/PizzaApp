document.addEventListener('DOMContentLoaded', () => {
    const pizzaForm = document.forms['pizza-form'];
    const totalElement = document.querySelector('output[name=total]')
    pizzaForm.reset();

    pizzaForm.addEventListener('input', () => {
        let sum = 0;
        pizzaForm.querySelectorAll('input.form-check-input').forEach(box => {
            if (box.checked) sum += +box.getAttribute('data-price');
        })

        pizzaForm.querySelectorAll('.form-select').forEach(select => {
            const option = select.options[select.selectedIndex]
            if (option) {
                console.log(option.getAttribute('data-price'));
                sum += +option.getAttribute('data-price');
                totalElement.textContent = sum.toFixed(2);
            }
        })
    });
    pizzaForm.onsubmit = e => {
        //e.preventDefault()  // disable submit
    }
})