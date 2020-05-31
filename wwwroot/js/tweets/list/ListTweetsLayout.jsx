import React from 'react'
import Tweet from './Tweet'

export default class ListTweetsLayout extends React.Component {

    constructor(props) {
        super(props)
        this.profileId = document.getElementById("profileId").dataset.profileId

        this.state = {
            tweets: []
        }

        this.fetchTweets = this.fetchTweets.bind(this)
        this.renderTweets = this.renderTweets.bind(this)
    }

    componentDidMount() {
        this.fetchTweets(this.profileId)
        window.addEventListener('tweet-created', e => {this.fetchTweets(this.profileId)})
    }

    async fetchTweets() {
        const req = {
            method: 'GET',
            headers: {
                'Content-type': 'application/json'
            }
        }

        let res = await fetch(`/tweets/getTweetsBy/${this.profileId}`, req)

        if (res.status !== 200) {
            alert('Something went wrong')
            return
        }

        res = await res.json()

        this.setState({tweets: res})
    }

    renderTweets() {
        const tweets = this.state.tweets.map(t => {
            return <Tweet tweet={t}/>
        })

        return <div className="list-tweet-container">{tweets}</div>
    }

    render() {
        return <div>
            {this.renderTweets()}
        </div>
    }
}