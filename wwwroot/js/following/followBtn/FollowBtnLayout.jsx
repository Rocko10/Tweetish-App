import React from 'react'

export default class FollowBtnLayout extends React.Component {
    //TODO: Check if a following exist

    constructor(props) {
        super(props)

        this.state = {
            activeBtn: true
        }

        this.sendFollow = this.sendFollow.bind(this)
        this.sendCheckExistFollowing = this.sendCheckExistFollowing.bind(this)

        this.profileId = document.getElementById('profileId') ? document.getElementById('profileId').dataset.profileId : null        
        this.userId = document.getElementById('userId') ? document.getElementById('userId').dataset.userId : null
    }
    
    componentDidMount() {
        this.sendCheckExistFollowing()
    }

    async sendFollow() {
        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({followerId: this.userId, followeeId: this.profileId})
        }

        let res = await fetch('/followings/create', req)

        if (res.status !== 200) {
            alert('Something went wrong')
        }

        this.setState({activeBtn: false})
    }

     async sendCheckExistFollowing() {
        const req = {
            method: 'GET',
            'Content-Type': 'application/json'
        }

        let exist = await fetch(`/followings/exist/${this.userId}/${this.profileId}`, req)
        exist = await exist.json()

        if (exist) {
            this.setState({activeBtn: false})
        }

     }

    render() {
        return <button
        disabled={!this.state.activeBtn}
        onClick={this.sendFollow}>
            Follow 
        </button>
    }

}