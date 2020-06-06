import React from 'react'
import Tweet from './Tweet'

export default class ListTweetsLayout extends React.Component {

    constructor(props) {
        super(props)
        this.profileId = document.getElementById("profileId").dataset.profileId
        this.userId = document.getElementById('userId').dataset.userId

        this.state = {
            tweets: [],
            reactions: []
        }

        this.fetchTweets = this.fetchTweets.bind(this)
        this.renderTweets = this.renderTweets.bind(this)
        this.fetchReactions = this.fetchReactions.bind(this)
        this.fetchRetweets = this.fetchRetweets.bind(this)
    }

    componentDidMount() {
        this.fetchTweets(this.profileId)
        this.fetchReactions()
        this.fetchRetweets()
        window.addEventListener('tweet-created', e => {this.fetchTweets(this.profileId)})
    }

    fetchTweets() {
        fetch(`/tweets/getTweetsBy/${this.profileId}`)
        .then(res => res.json())
        .then(tweets => {
            this.setState({tweets})
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
            this.setState({reactions})
        })
    }

    renderTweets() {
        const tweets = this.state.tweets.map(t => {
            return <Tweet tweet={t} userId={this.userId} reactions={this.state.reactions}/>
        })

        return <div className="list-tweet-container">{tweets}</div>
    }

    render() {
        return <div>
            {this.renderTweets()}
        </div>
    }
}