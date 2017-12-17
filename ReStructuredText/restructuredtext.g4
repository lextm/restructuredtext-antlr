grammar ReStructuredText;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  language=CSharp;
}

parse
  :  (element | empty_line) + EOF
  ;

element
  :  comment | paragraph | line_block
  ;

comment
  :  Comment line+
  ;

paragraph
  :  line+
  ;
  
line_block
  :  (Block line)+
  ;  

line
  :  indentation? text
  ;

empty_line
  :  Space* LineBreak
  ;

indentation
  :  Space+
  ;

text
  : text_fragment_start text_fragment* LineBreak
  ;
 
text_fragment_start
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  Space
  |  Star
  |  EscapeSequence
  |  Any
  ;

text_fragment
  : text_fragment_start
  | Block
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ Star
  ;

boldAtom
  :  ~(Star | LineBreak)
  |  italic
  ;

italicAtom
  :  ~(Star | LineBreak)
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~BackTick+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

Block
  :  '| '
  ;

Comment
  :  '..'
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;