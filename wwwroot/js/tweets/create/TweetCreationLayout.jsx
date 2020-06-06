import React from 'react'

export default class TweetCreationLayout extends React.Component {

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
        window.dispatchEvent(new Event('tweet-created'))
    }

    render() {
        if (!this.userId) {
            return <p>Login to start tweeting</p>
        }

        return <div>
            <label>Tweet</label>
            <input 
                value={this.state.text}
                onChange={e => {this.updateField(e.target.value)}}
            />
            <button onClick={this.sendCreate}>
                Create
            </button>
        </div>
    }

}