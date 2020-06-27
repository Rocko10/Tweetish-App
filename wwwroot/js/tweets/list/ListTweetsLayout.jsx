import React from 'react'
import Tweet from './Tweet'

export default class ListTweetsLayout extends React.Component {

    constructor(props) {
        super(props)
        this.profileId = document.getElementById("profileId").dataset.profileId
        this.userId = document.getElementById('userId').dataset.userId

        this.state = {
            tweets: [],
            reactions: [],
            userTweetReactions: []
        }

        this.fetchTweets = this.fetchTweets.bind(this)
        this.renderTweets = this.renderTweets.bind(this)
        this.fetchReactions = this.fetchReactions.bind(this)
        this.fetchRetweets = this.fetchRetweets.bind(this)
        this.fetchUserReactions = this.fetchUserReactions.bind(this)
        this.reactedToTweet = this.reactedToTweet.bind(this)
    }

    componentDidMount() {
        this.fetchTweets(this.profileId)
        this.fetchRetweets()
        window.addEventListener('tweet-created', e => {this.fetchTweets(this.profileId)})
        window.addEventListener('tweets-fetched', e => {this.fetchReactions()})
        window.addEventListener('reactions-fetched', e => {this.fetchUserReactions()})
    }

    fetchTweets() {
        fetch(`/tweets/getTweetsBy/${this.profileId}`)
        .then(res => res.json())
        .then(tweets => {
            this.setState({tweets: tweets.concat(this.state.tweets)}, () => {window.dispatchEvent(new Event('tweets-fetched'))})
        })
    }

    fetchRetweets() {
        fetch(`/retweets/getByUserId/${this.profileId}`)
        .then(res => res.json())
        .then(retweets => {
            let tweets = this.state.tweets
            this.setState({tweets: tweets.concat(retweets)})
        })
    }

    fetchReactions() {
        fetch('/reactions/getAll')
        .then(res => res.json())
        .then(reactions => {
            this.setState({reactions}, () => {window.dispatchEvent(new Event('reactions-fetched'))})
        })
    }

    fetchUserReactions() {
        const tweets = this.state.tweets
        const reactions = this.state.reactions
        const userId = this.profileId
        let userReactions = []

        for (const t of tweets) {
            for (const r of reactions) {
                userReactions.push(
                    {userId, tweetId: t.id, reactionId: r.id}
                );
            }
        } 

        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userReactions)
        }

        fetch(`/userTweetReaction/reactedToMany`, req)
        .then(res => res.json())
        .then(userTweetReactions => 
            this.setState({userTweetReactions}, () => { window.dispatchEvent(new Event('user-tweet-reactions-fetched')) }))
    }

    reactedToTweet(profileId, tweetId, reactionId) {
        const reactions = this.state.userTweetReactions
        for (const r of reactions) {
            if (r.userId == profileId && r.tweetId == tweetId && r.reactionId == reactionId) {
                return r.reacted
            }
        }
    }

    renderTweets() {
        const tweets = this.state.tweets.map(t => {
            return <Tweet 
            tweet={t} 
            userId={this.userId} 
            reactions={this.state.reactions}
            reactedToTweet={this.reactedToTweet}
            />
        })

        return <div className="list-tweet-container">{tweets}</div>
    }

    render() {
        return <div>
            {this.renderTweets()}
        </div>
    }

}