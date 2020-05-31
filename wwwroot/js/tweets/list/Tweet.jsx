import React from 'react'

export default class Tweet extends React.Component {
    
    constructor(props) {
        super(props)
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
                <button>Retweet</button>
            </div>
        </div>
    }

}