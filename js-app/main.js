const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            for (let i of beanVarieties){
                document.querySelector('.varietyList').innerHTML += `<p>${i.name}</p>`
            };
        })
});

const saveButton = document.querySelector("#savebtn");
saveButton.addEventListener("click", event => {
    event.preventDefault();
    const name = document.querySelector("#name").value
    const region = document.querySelector("#region").value
    const notes = document.querySelector("#notes").value

    const bvObj = {
        name: name,
        region: region,
        notes: notes
    }
    return fetch(url , {
        method: "POST" , 
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(bvObj)
    })
    .then(resp => resp.json())
    .then(()=>{
        document.querySelector("#name").value = ""
        document.querySelector("#region").value = ""
        document.querySelector("#notes").value = ""
    })

} )

function getAllBeanVarieties() {
    return fetch(url).then(resp => resp.json());
}

