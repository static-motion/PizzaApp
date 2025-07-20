document.addEventListener('DOMContentLoaded', () => {
    const pizzaForm = document.forms['create-pizza'];

    pizzaForm.reset();

    pizzaForm.oninput = _ => {
        let sum = 0;

        pizzaForm.querySelectorAll('input').forEach(box => {
            if (box.checked) sum += +box.getAttribute('data-price');
        })

        pizzaForm.querySelectorAll('select').forEach(select => {
            const val = select.value;
            const option = select[val]
            if (option) {
                console.log(option.getAttribute('data-price'));
                sum += +option.getAttribute('data-price');
            }
        })

        pizzaForm.total.textContent = `$${sum}`
    }
    pizzaForm.onsubmit = e => {
        //e.preventDefault()  // disable submit
    }
});