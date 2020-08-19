import React from 'react';
import { ArticleModel } from 'models';
import { FixedSizeList, ListChildComponentProps } from 'react-window';
import Article from '../Article/Article';

interface FixedSizeListRenderFnProps extends ListChildComponentProps {
  // eslint-disable-next-line react/no-unused-prop-types
  data: ArticleModel[];
}

const articleStyle: React.CSSProperties = {
  display: 'flex',
  justifyContent: 'flex-start',
};

const containerStyle: React.CSSProperties = {
  width: '40%',
};

interface ArticleListProps {
  articles: ArticleModel[];

  articleRef: (node?: Element | null | undefined) => void;
}

const ArticleList: React.FC<ArticleListProps> = ({ articles, articleRef }) => {
  return (
    <div style={containerStyle}>
      <div>Najnovije</div>
      <FixedSizeList
        height={800}
        itemData={articles}
        itemCount={articles.length}
        itemSize={120}
        overscanCount={5}
        width="100%"
        style={{ margin: '10px' }}
      >
        {({ data, index, style }: FixedSizeListRenderFnProps) => {
          const article = data[index];

          if (!article) {
            return null;
          }

          return (
            <Article
              key={article.id}
              article={article}
              style={{ ...articleStyle, ...style }}
              ref={articles.length - index === 5 ? articleRef : undefined}
            />
          );
        }}
      </FixedSizeList>
    </div>
  );
};

export default ArticleList;
