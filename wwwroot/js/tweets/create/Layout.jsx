import React from 'react'

export default class Layout extends React.Component {

    constructor(props){
        super(props)

        this.userId = document.getElementById('userId').dataset.userId

        this.state = {
            text: ''
        }

        this.updateField = this.updateField.bind(this)
        this.sendCreate = this.sendCreate.bind(this)
    }

    updateField(value){
        this.setState({text: value})
    }

    async sendCreate(){
        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                text: this.state.text,
                userId: this.userId
            })
        }

        let res = await fetch('/tweets/create', req)

        if (res.status != 200) {
            alert('An error ocurred')
        }
        
        this.setState({text: ''})
    }

    render() {
        if (!this.userId) {
            return null
        }

        return <div>
            <label>Tweet </label>
            <input value={this.state.text} onChange={e => {this.updateField(e.target.value)}} type="text"/>
            <button onClick={this.sendCreate}>Tweet</button>
        </div>
    }

}