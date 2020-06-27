import React from 'react'
import ReactionBtn from './ReactionBtn'

export default class Tweet extends React.Component {
    
    constructor(props) {
        super(props)

        this.sendRetweet = this.sendRetweet.bind(this)
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
        return this.props.reactions
        .map(r => <ReactionBtn 
            reaction={r}
            userId={this.props.userId}
            tweet={this.props.tweet}
            reactedToTweet={this.props.reactedToTweet}
        />)
    }

    sendIsRetweeted() {
        fetch(`/retweets/isRetweeted/${this.props.userId}/${this.props.tweet.id}`)
        .then(res => res.json())
        .then(isRetweeted => {
            this.setState({isRetweeted})
        })
    }

    handleClickComment(tweet) {
        window.dispatchEvent(new CustomEvent('selectedComment', {detail: {tweetId: tweet.id, text: tweet.text} }))
    }

    render() {
        const tweet = this.props.tweet

        return <div className="tweet">
            <span className="nickname"> {tweet.nickname} </span>
            <span className="createdAt"> {tweet.createdAt} </span>
            <p>
                {tweet.text} 
            </p>
            <div>
                <button onClick={this.sendRetweet}>
                    { this.state.isRetweeted ? "Un-Retweet" : "Retweet" }
                </button>
                {this.renderReactions()}
                <button onClick={e => {this.handleClickComment(this.props.tweet)}}>Comment</button>
            </div>
        </div>
    }

}