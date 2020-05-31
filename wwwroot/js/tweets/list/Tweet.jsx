import React from 'react'

export default class Tweet extends React.Component {
    
    constructor(props) {
        super(props)

        this.sendRetweet = this.sendRetweet.bind(this)
    }

    async sendRetweet() {
        let res = await fetch(`/retweets/toggle/${this.props.userId}/${this.props.tweet.id}`)

        if (res.status == 400) {
            alert('Cannot retweet your tweet')

            return
        }

        alert('Succesful retweet toggle')
    }

    render() {
        const tweet = this.props.tweet

        return <div className="tweet">
            <span className="nickname"> {tweet.nickname} </span>
            <span className="createdAt"> {tweet.createdAt} </span>
            <p>
                {tweet.text} 
            </p>
            <div className="controls">
                <button onClick={this.sendRetweet}>Retweet</button>
            </div>
        </div>
    }

}