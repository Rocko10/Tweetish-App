import React from 'react'

export default class Tweet extends React.Component {
    
    constructor(props) {
        super(props)

        this.sendRetweet = this.sendRetweet.bind(this)
        this.sendToggleUserTweetReaction = this.sendToggleUserTweetReaction.bind(this)
        this.sendIsRetweeted = this.sendIsRetweeted.bind(this)

        this.state = {
            isRetweeted: false
        }
    }

    componentDidMount() {
        this.sendIsRetweeted()
    }

    sendRetweet() {
        fetch(`/retweets/toggle/${this.props.userId}/${this.props.tweet.id}`)
        .then(res => {
            if (res.status == 400) {
                alert('Cannot retweet your tweet')

                return
            }

            alert('Succesful retweet toggle')
        })
    }

    renderReactions() {
        return this.props.reactions.map(r => {
            let text = 'favorite'

            if (r.name == 'Star') {
                text = 'Save'
            }

            return <button onClick={e => this.sendToggleUserTweetReaction(this.props.userId, this.props.tweet.id, r.id)}>
                {text}
            </button>
        })
    }

    sendToggleUserTweetReaction(userId, tweetId, reactionId) {
        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({userId, tweetId, reactionId})
        }

        let res = fetch('/userTweetReaction/toggle', req)
        res.then(data => {
            if (data.status == 200) {
                alert('Reaction toggled succesfully')
            }
        })
    }

    sendIsRetweeted() {
        fetch(`/retweets/isRetweeted/${this.props.userId}/${this.props.tweet.id}`)
        .then(res => res.json())
        .then(isRetweeted => {
            this.setState({isRetweeted})
        })
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
                <button onClick={this.sendRetweet}>
                    { this.state.isRetweeted ? "Un-Retweet" : "Retweet" }
                </button>
                {this.renderReactions()}
            </div>
        </div>
    }

}