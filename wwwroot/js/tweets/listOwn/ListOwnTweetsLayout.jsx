import React from 'react'

export default class ListOwnTweetsLayout extends React.Component {

    constructor(props) {
        super(props)
        this.userId = document.getElementById("userId").dataset.userId

        this.state = {
            tweets: []
        }

        this.fetchOwnTweets = this.fetchOwnTweets.bind(this)
        this.renderTweets = this.renderTweets.bind(this)
    }

    componentDidMount() {
        this.fetchOwnTweets(this.userId)
    }

    async fetchOwnTweets() {
        const req = {
            method: 'GET',
            headers: {
                'Content-type': 'application/json'
            }
        }

        let res = await fetch(`/tweets/getTweetsBy/${this.userId}`, req)

        if (res.status !== 200) {
            alert('Something went wrong')
            return
        }

        res = await res.json()

        this.setState({tweets: res})
    }

    renderTweets() {
        const tweets = this.state.tweets.map(t => {
            return <p>{t.text}</p>
        })

        return <div className="list-tweet-container">{tweets}</div>
    }

    render() {
        return <div>
            {this.renderTweets()}
        </div>
    }
}