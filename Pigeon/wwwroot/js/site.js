// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const dataMiningForm = document.querySelector('#data-mining-form')
const dataMiningInput = document.querySelector('#data-mining-input')

dataMiningForm.addEventListener('submit',async(event)=>{
    event.preventDefault()
    const requestToken = dataMiningForm.children[1].value
    const result = await fetch("/DataMining",{method:'POST',  headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },body:JSON.stringify({
        Input: dataMiningInput.value,
        __RequestVerificationToken:requestToken
    })})
    
    const recivedString = result.join(',')
    dataMiningInput.value += recivedString
})